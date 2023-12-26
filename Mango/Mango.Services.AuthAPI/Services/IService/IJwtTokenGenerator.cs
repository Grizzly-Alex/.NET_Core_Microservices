using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Services.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser user);
    }
}
