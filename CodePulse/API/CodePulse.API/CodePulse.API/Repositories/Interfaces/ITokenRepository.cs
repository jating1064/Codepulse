using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string createJwtToken(IdentityUser user, List<string> roles);
    }
}
