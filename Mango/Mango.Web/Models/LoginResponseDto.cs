namespace Mango.Web.Models
{
    public sealed class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
