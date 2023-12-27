using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;                
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginRequestDto());

        [HttpGet]
        public IActionResult Register() => View(new RegistrationRequestDto());

        [HttpPost]
        public IActionResult Logout() 
        {
            return View();
        }

    }
}
