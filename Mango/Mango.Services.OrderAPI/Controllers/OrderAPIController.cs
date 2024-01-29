using AutoMapper;
using Mango.OrderAPI.Utility;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;


namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IProductService _productService;

        public OrderAPIController(IMapper mapper, AppDbContext db, IProductService productService)
        {
            _response = new ResponseDto();
            _mapper = mapper;
            _db = db;
            _productService = productService;
            StripeConfiguration.ApiKey = SD.StripeSessionKey;
        }

         
        [Authorize]
        [HttpPost("create_order")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.Time = DateTime.UtcNow;
                orderHeaderDto.Status = SD.StatusPending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);
                OrderHeader orderCreated = _db.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                
                await _db.SaveChangesAsync();

                orderHeaderDto.Id = orderCreated.Id;
                _response.Result = orderHeaderDto;
            }
            catch (Exception ex) 
            {
                _response.Result = false;
                _response.Message = ex.Message; 
            }
            return _response;
        }

        [Authorize]
        [HttpPost("create_stripe_session")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    Mode = "payment",
                    LineItems = new List<SessionLineItemOptions>(),
                    Discounts = stripeRequestDto.OrderHeader.Discount > 0
                        ? new List<SessionDiscountOptions>()
                        {
                            new SessionDiscountOptions { Coupon = stripeRequestDto.OrderHeader.CouponCode }
                        } 
                        : null
                };

                stripeRequestDto.OrderHeader.OrderDetails.ToList()
                    .ForEach(item => options.LineItems.Add(new SessionLineItemOptions
                    {
                        Quantity = item.Count,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)item.Price * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            },
                        },
                    }));

                var service = new SessionService();
                Session  session = service.Create(options);
                stripeRequestDto.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeaders.First(orderHeader => orderHeader.Id == stripeRequestDto.OrderHeader.Id);
                orderHeader.StriprSessionId = session.Id;
                await _db.SaveChangesAsync();

                _response.Result = stripeRequestDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        [Authorize]
        [HttpPost("validate_stripe_session")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(orderHeader => orderHeader.Id == orderHeaderId);
                SessionService service = new();
                Session session = service.Get(orderHeader.StriprSessionId);
                PaymentIntentService paymentintentService = new();
                PaymentIntent paymentIntent = paymentintentService.Get(session.PaymentIntentId);

                if(paymentIntent.Status.Equals("succeeded", StringComparison.OrdinalIgnoreCase))
                {
                    //then payment was successful
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.StatusApproved;
                    _db.SaveChanges();

                    _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

    }
}
