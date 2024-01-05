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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            AppDbContext db,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;    
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            AppUser user = _db.Users.FirstOrDefault(u => string.Equals(u.Email.ToUpper(), email.ToUpper()));

            bool result = user != null;

            if (result) 
            { 
                if(!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);
            }

            return result;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.Users.FirstOrDefault(u => string.Equals(u.UserName.ToUpper(), loginRequestDto.UserName.ToUpper()));
            if (user is null) return new() { Token = string.Empty};
                
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isValid) return new() { Token = string.Empty };

            return new()
            {
                User = new()
                {
                    ID = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber
                },
                Token = _jwtTokenGenerator.GenerateToken(user, await _userManager.GetRolesAsync(user)),
            };  
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
