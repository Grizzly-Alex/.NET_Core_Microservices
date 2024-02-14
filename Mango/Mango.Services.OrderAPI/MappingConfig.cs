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
                .ForMember(dest => dest.Id, config => config.Ignore())
                .ReverseMap();

                config.CreateMap<CartHeaderDto, OrderHeaderDto>()
                .ForMember(dest => dest.Id, config => config.Ignore());

                config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, config => config.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, config => config.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Id, config => config.Ignore());

                config.CreateMap<OrderDetailsDto, CartDetailsDto>()
                .ForMember(dest => dest.Id, config => config.Ignore());

                config.CreateMap<OrderHeader, OrderHeaderDto>()
                .ReverseMap();

                config.CreateMap<OrderDetailsDto, OrderDetails>()
                .ReverseMap();                
            });
        }
    }
}