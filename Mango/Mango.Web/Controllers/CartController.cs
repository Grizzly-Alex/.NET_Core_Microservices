﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;


namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService couponService, IOrderService orderService)
        {
            _cartService = couponService;   
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> Index() 
        { 
            return View(await LoadCartDtoBasedOnLoggedInUser()); 
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        
        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;   
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var response = await _orderService.CreateOrderAsync(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if (response != null && response.IsSuccess)
            {
                //get stripe session and redirect to stripe to place order
                var domain = $"{Request.Scheme}://{Request.Host.Value}/";

                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = $"{domain}{RouteData.Values["controller"]}/{nameof(Confirmation)}?orderId={orderHeaderDto.Id}",
                    CancelUrl = $"{domain}{RouteData.Values["controller"]}/{nameof(Checkout)}",
                    OrderHeader = orderHeaderDto,
                };

                var stripeResponse = await _orderService.CreateStripeSessionAsync(stripeRequestDto);
                var stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(stripeResponse.Result));
                Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                return new StatusCodeResult((int)HttpStatusCode.RedirectMethod);
            }

            return RedirectToAction(nameof(Checkout));
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            ResponseDto? response = await _orderService.ValidateStripeSessionAsync(orderId);

            if (response is not null && response.IsSuccess)
            {
                OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

                if (orderHeader.Status.Equals(SD.StatusApproved, StringComparison.OrdinalIgnoreCase))
                {
                    return View(orderId);
                }
            }
            //redirect to error page
            return View(orderId);
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            ResponseDto? response = await _cartService.RemoveFromCartAsync(cartDetailsId);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated Successfully";
            }
            else
            {
                TempData["error"] = "Something went wrong";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);

            if (response is not null && response.IsSuccess && response.Result is true)
            {
                TempData["success"] = "Cart updated Successfully";
            }
            else
            {
                TempData["error"] = "Something went wrong";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = string.Empty;

            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated Successfully";
            }
            else
            {
                TempData["error"] = "Something went wrong";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(user => user.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.EmailCart(cart);

            if (response is not null && response.IsSuccess && response.Result is true)
            {
                TempData["success"] = "Email will be processed and sent shortly.";
            }
            else
            {
                TempData["error"] = "Something went wrong";
            }

            return RedirectToAction(nameof(Index));
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
