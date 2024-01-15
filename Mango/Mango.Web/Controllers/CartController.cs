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
        public async Task<IActionResult> Index() 
        { 
            return View(await LoadCartDtoBasedOnLoggedInUser()); 
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.RemoveFromCartAsync(cartDetailsId);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(user => user.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);

            return response is not null && response.IsSuccess 
                ? JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result)) 
                : new CartDto();
        }
    }
}
