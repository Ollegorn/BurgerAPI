using Microsoft.AspNetCore.Identity;
using ServiceContracts.AuthorizationDto;
using System.Security.Claims;

namespace ServiceContracts.Interfaces
{
    public interface IJwtService
    {
        public Task<AuthResultDto> GenerateJwtToken(IdentityUser user);
        public Task<List<Claim>> GetAllValidClaims(IdentityUser user);
    }
}
