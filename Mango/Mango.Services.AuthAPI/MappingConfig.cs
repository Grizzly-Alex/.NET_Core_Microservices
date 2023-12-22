using AutoMapper;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI
{
    public sealed class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<AppUser, RegistrationRequestDto>()
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Email.ToLower(), act => act.MapFrom(src => src.NormalizedEmail))
                    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                    .ForMember(dest => dest.PhoneNumber, act => act.MapFrom(src => src.PhoneNumber))
                    .ReverseMap();

                config.CreateMap<AppUser, UserDto>()
                    .ForMember(dest => dest.ID, act => act.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                    .ForMember(dest => dest.PhoneNumber, act => act.MapFrom(src => src.PhoneNumber))
                    .ReverseMap();

            });
        }
    }
}
