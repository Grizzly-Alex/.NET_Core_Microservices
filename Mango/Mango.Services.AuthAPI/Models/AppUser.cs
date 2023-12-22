using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Models
{
    public sealed class AppUser : IdentityUser
    {
         public string Name { get; set; }
    }
}
