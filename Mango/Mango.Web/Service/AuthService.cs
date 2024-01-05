using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;               
        }

        public async Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole",
            });
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = SD.AuthAPIBase + "/api/auth/login",
            }, withBearer: false);
        }

        public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = SD.AuthAPIBase + "/api/auth/register",
            }, withBearer: false);
        }
    }
}
