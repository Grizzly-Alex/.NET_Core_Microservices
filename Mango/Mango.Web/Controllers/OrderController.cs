using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;


namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;               
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OrderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
            string userId = User.Claims.Where(user => user.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto response = await _orderService.GetOrderAsync(orderId);

            if (response != null)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            }

            if (!User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }

            return View(orderHeaderDto);
        }

        [HttpGet]
        public IActionResult GetAll(string status) 
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = string.Empty;

            if (!User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(user => user.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }
            ResponseDto response = _orderService.GetAllOrdersAsync(userId).GetAwaiter().GetResult();

            if (response != null)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));

                switch (status)
                {
                    case "approved": list = list.Where(u => u.Status.Equals(SD.StatusApproved, StringComparison.OrdinalIgnoreCase)); break;
					case "readyforpickup": list = list.Where(u => u.Status.Equals(SD.StatusReadyForPickup, StringComparison.OrdinalIgnoreCase)); break;
					case "canceled": list = list.Where(u => u.Status.Equals(SD.StatusCancelled, StringComparison.OrdinalIgnoreCase)); break;
                    default: break;
				}
            }
            else
            {
                list = new List<OrderHeaderDto>();  
            }
            return Json(new {data = list });
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, SD.StatusReadyForPickup);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated seccessfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, SD.StatusCompleted);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated seccessfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, SD.StatusCancelled);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated seccessfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }
    }
}
