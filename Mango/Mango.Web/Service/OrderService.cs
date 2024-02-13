using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public sealed class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateOrderAsync(CartDto cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cart,
                Url = SD.OrderAPIBase + "/api/order/create_order",
            });
        }

        public async Task<ResponseDto> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrderAPIBase + "/api/order/create_stripe_session",
            });
        }

        public async Task<ResponseDto> GetAllOrdersAsync(string? userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderAPIBase + $"/api/order/get_orders/{userId}",
            });
        }

        public async Task<ResponseDto> GetOrderAsync(int orderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderAPIBase + $"/api/order/get_order/{orderId}",
            });
        }

        public async Task<ResponseDto> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = newStatus,
                Url = SD.OrderAPIBase + $"/api/order/update_order_status/{orderId}",
            });
        }

        public async Task<ResponseDto> ValidateStripeSessionAsync(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = orderHeaderId,
                Url = SD.OrderAPIBase + "/api/order/validate_stripe_session",
            });
        }
    }
}
