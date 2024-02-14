namespace Mango.Services.AuthAPI.Models.Dto
{
    public sealed class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
