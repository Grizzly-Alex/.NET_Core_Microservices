using Mango.Web.Models;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
    public sealed class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public Task<ResponseDto> CreateCouponAsync(CouponDto coupon)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> DeleteCouponAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResponseDto>> GetAllCouponsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetCouponByCodeAsync(string couponCode)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateCouponAsync(CouponDto coupon)
        {
            throw new NotImplementedException();
        }
    }
}
