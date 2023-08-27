using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceContracts.AuthorizationDto;
using ServiceContracts.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BurgerDbContext _dbContext;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger<JwtService> _logger;
        public JwtService(IConfiguration configuration, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, BurgerDbContext burgerDbContext, TokenValidationParameters tokenValidationParameters,ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = burgerDbContext;
            _tokenValidationParameters = tokenValidationParameters;
            _logger = logger;
        }
        public async Task<TokensResponseDto> GenerateJwtToken(IdentityUser user)
        {
            _logger.LogInformation("Generating jwt token");

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Key").Value);

            var claims =  GetAllValidClaims(user).GetAwaiter().GetResult();

            //token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtSettings:ExpiryTimeframe").Value)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor); //creating token
            var jwtToken = jwtTokenHandler.WriteToken(token); //converting to string

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                Token = GenerateRefreshToken(), //generate a refresh token
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                IsRevoked = false,
                UserId = user.Id

            };
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            var tokensResponse = new TokensResponseDto { Token = jwtToken, RefreshToken = refreshToken.Token };

            _logger.LogInformation("Jwt generated successfully");

            return tokensResponse;
        }



        public async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
        {
            _logger.LogInformation("Getting all claims for user");

            var _options = new IdentityOptions();
            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            };


            //Getting claims that are assigned to user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            //get user role and add to claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {

                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    _logger.LogInformation("Adding role claims");

                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
                
            }

            _logger.LogInformation("Valid claims retrieved successfully");

            return claims;

        }

        public async Task<TokensResponseDto> VerifyAndGenerateToken(TokenRequestDto tokenRequest)
        {
            _logger.LogInformation("Verifying refresh token and generating new token");

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                _tokenValidationParameters.ValidateLifetime = false; //for testing

                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == null)
                    {
                        _logger.LogInformation("Checking cryptographic algorythm of token");

                        return null;
                    }
                }
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                _logger.LogInformation("Other validations for token");

                var now = DateTime.UtcNow;
                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                if (expiryDate > DateTime.Now)
                    return new TokensResponseDto { Errors = new List<string>() { "Expired token" } };

                var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                    return new TokensResponseDto { Errors = new List<string>() { "Token doesn't exist" } };

                if (storedToken.IsRevoked)
                    return new TokensResponseDto { Errors = new List<string>() { "Token is revoked" } };

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                    return new TokensResponseDto { Errors = new List<string>() { "Wrong id" } };

                if (storedToken.ExpiryDate < DateTime.UtcNow)
                    return new TokensResponseDto { Errors = new List<string>() { "Expired tokens" } };


                _dbContext.RefreshTokens.Update(storedToken);
                await _dbContext.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

                var finalToken = await GenerateJwtToken(dbUser);

                _logger.LogInformation("Refresh token validated successfully and refreshed jwt");

                return finalToken;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Some exception occured");

                return new TokensResponseDto { Errors = new List<string>{"Server error"}};
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            _logger.LogInformation("Creating unix time stamp");

            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
            _logger.LogInformation("Created unix time stamp successfully");

            return dateTimeVal;
        }


        private string GenerateRefreshToken()
        {
            _logger.LogInformation("Generating refresh token");

            byte[] bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);

            _logger.LogInformation("Generated refresh token successfully");

            return Convert.ToBase64String(bytes);
        }
    }
}
