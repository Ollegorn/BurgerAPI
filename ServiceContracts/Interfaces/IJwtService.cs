using Microsoft.AspNetCore.Identity;
using ServiceContracts.AuthorizationDto;
using System.Security.Claims;

namespace ServiceContracts.Interfaces
{
    public interface IJwtService
    {
        public Task<TokensResponseDto> GenerateJwtToken(IdentityUser user);
        public Task<List<Claim>> GetAllValidClaims(IdentityUser user);

        public Task<TokensResponseDto> VerifyAndGenerateToken(TokenRequestDto tokenRequest);
    }
}
