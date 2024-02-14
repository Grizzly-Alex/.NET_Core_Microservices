using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDto request);
        Task<ResponseDto> RegisterAsync(RegistrationRequestDto request);
        Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto request);
    }
}
