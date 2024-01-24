using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;


namespace Mango.Services.OrderAPI
{
    public sealed class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, config => config.MapFrom(src => src.Total))
                .ReverseMap();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, config => config.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, config => config.MapFrom(src => src.Product.Price))
                .ReverseMap();

                config.CreateMap<OrderHeader, OrderHeaderDto>()
                .ReverseMap();

                config.CreateMap<OrderDetailsDto, OrderDetails>()
                .ReverseMap();
                
            });
        }
    }
}