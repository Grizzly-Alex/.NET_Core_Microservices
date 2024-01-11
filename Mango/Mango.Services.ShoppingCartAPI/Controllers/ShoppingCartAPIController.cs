using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public ShoppingCartAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(header => header.UserId.Equals(cartDto.CartHeader.UserId));
                if(cartHeaderFromDb == null)
                {
                    //create cart header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.Id;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cartDetails => 
                            cartDetails.ProductId == cartDto.CartDetails.First().ProductId && 
                            cartDetails.CartHeaderId == cartHeaderFromDb.Id);
                    if(cartDetailsFromDb == null)
                    {
                        //create new cart details
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().Id = cartDetailsFromDb.Id;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;    
            }

            return _response;
        }

    }
}
