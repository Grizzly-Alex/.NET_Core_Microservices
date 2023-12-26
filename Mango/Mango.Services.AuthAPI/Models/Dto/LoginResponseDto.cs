namespace Mango.Services.AuthAPI.Models.Dto
{
    public sealed class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
