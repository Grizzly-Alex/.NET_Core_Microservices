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
                new() { Value = SD.RoleAdmin, Text = SD.RoleAdmin },
                new() { Value = SD.RoleCustomer, Text = SD.RoleCustomer },
            };

            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assingRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = SD.RoleCustomer;
                }

                assingRole = await _authService.AssignRoleAsync(obj);

                if (assingRole != null && assingRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }


            var roleList = new List<SelectListItem>()
            {
                new() { Value = SD.RoleAdmin, Text = SD.RoleAdmin },
                new() { Value = SD.RoleCustomer, Text = SD.RoleCustomer },
            };

            ViewBag.RoleList = roleList;

            return View(obj);
        }

        [HttpPost]
        public IActionResult Logout() 
        {
            return View();
        }
    }
}
