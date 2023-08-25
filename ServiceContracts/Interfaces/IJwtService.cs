using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ServiceContracts.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(IdentityUser user);
        public Task<List<Claim>> GetAllValidClaims(IdentityUser user);
    }
}
