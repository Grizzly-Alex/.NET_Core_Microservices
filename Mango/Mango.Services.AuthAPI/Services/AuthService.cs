using AutoMapper;
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AuthService(
            AppDbContext db,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _db = db;    
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;   
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.Users.FirstOrDefault(u => string.Equals(u.Name.ToUpper(), loginRequestDto.UserName.ToUpper()));

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user is null || !isValid) 
            {
                return new LoginResponseDto() { User = null, Token = string.Empty};
            }

            UserDto userDto = new()
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = string.Empty,
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            AppUser user = _mapper.Map<AppUser>(registrationRequestDto);

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _db.Users.First(user => user.UserName == registrationRequestDto.Email);
                    UserDto userDto = _mapper.Map<UserDto>(userToReturn);

                    return string.Empty;
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
