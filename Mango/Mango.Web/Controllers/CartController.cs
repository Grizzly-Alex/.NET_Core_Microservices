using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService couponService)
        {
            _cartService = couponService;              
        }

        [Authorize]
        public async Task<IActionResult> Index() => View(await LoadCartDtoBasedOnLoggedInUser());


        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(user => user.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);

            return response != null & response.IsSuccess 
                ? JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response)) 
                : new CartDto();
        }
    }
}
