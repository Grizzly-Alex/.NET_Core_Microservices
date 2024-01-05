using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;  
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginRequestDto());

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto response = await _authService.LoginAsync(obj);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = response.Message;
                return View(obj);
            }
        }

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
            else
            {
                TempData["error"] = result.Message;
            }


            var roleList = new List<SelectListItem>()
            {
                new() { Value = SD.RoleAdmin, Text = SD.RoleAdmin },
                new() { Value = SD.RoleCustomer, Text = SD.RoleCustomer },
            };

            ViewBag.RoleList = roleList;

            return View(obj);
        }

        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);

            List<Claim> claims = new()
            {
                new (JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(i => i.Type.Equals(JwtRegisteredClaimNames.Email)).Value),
                new (JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(i => i.Type.Equals(JwtRegisteredClaimNames.Name)).Value),
                new (JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(i => i.Type.Equals(JwtRegisteredClaimNames.Sub)).Value),
                new (ClaimTypes.Name, jwt.Claims.FirstOrDefault(i => i.Type.Equals(JwtRegisteredClaimNames.Email)).Value),
                new (ClaimTypes.Role, jwt.Claims.FirstOrDefault(i => i.Type.Equals("role")).Value),
            };

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(claims);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}
