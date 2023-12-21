using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

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
            var response = await _couponService.GetAllCouponsAsync();

            var list = response != null && response.IsSuccess
                ? JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result))
                : new List<CouponDto>();

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponDto model)
        {
            if(ModelState.IsValid)
            {
                var response = await _couponService.CreateCouponAsync(model);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.GetCouponByIdAsync(id);

                if (response != null && response.IsSuccess)
                {
                    var model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

                    return View(model);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CouponDto model)
        {            
            var response = await _couponService.DeleteCouponAsync(model.CouponId);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }            
            return View(model);
        }
    }
}
