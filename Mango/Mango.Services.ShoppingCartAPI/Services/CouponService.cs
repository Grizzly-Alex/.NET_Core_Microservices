using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public sealed class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCouponAsync(string couponCode)
        {
            HttpClient client = _httpClientFactory.CreateClient("Coupon");
            HttpResponseMessage response = await client.GetAsync($"/api/coupon/get_by_code/{couponCode}");
            string apiContent = await response.Content.ReadAsStringAsync();
            ResponseDto responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }

            return responseDto.IsSuccess
                ? JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result))
                : new CouponDto();
        }
    }
}
