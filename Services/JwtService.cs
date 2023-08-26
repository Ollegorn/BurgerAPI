using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceContracts.AuthorizationDto;
using ServiceContracts.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BurgerDbContext _dbContext;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public JwtService(IConfiguration configuration, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, BurgerDbContext burgerDbContext, TokenValidationParameters tokenValidationParameters)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = burgerDbContext;
            _tokenValidationParameters = tokenValidationParameters;
        }
        public async Task<AuthResultDto> GenerateJwtToken(IdentityUser user)
        {
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
                Token = RandomStringGeneration(22), //generate a refresh token
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id

            };
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            var result = new AuthResultDto
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };
            return result;
        }



        public async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
        {
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
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
                
            }
            return claims;




        }

        private static string RandomStringGeneration(int length)
        {
            var random = new Random();
            var chars = "QWERTYUIOPLKJHGFDSAZXCVBNM1234567890qazxswedcvfrtgbnhyujmkiolp_";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
