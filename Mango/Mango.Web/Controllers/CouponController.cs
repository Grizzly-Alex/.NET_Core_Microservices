using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;              
        }

        public async Task<IActionResult> Index()
        {
            ResponseDto response = await _couponService.GetAllCouponsAsync();

            var list = response != null && response.IsSuccess
                ? JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result))
                : new List<CouponDto>();

            return View(list);
        }
    }
}
