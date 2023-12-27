using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Register() 
        {
            var roleList = new List<SelectListItem>()
            {
                new() { Value = SD.Role.Admin.ToString(), Text = SD.Role.Admin.ToString() },
                new() { Value = SD.Role.Customer.ToString(), Text = SD.Role.Customer.ToString() },
            };

            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public IActionResult Logout() 
        {
            return View();
        }
    }
}
