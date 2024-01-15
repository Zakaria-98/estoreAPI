using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using storeAPI.Dto;
using storeAPI.Helpers;
using storeAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace storeAPI.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;

        private readonly JWT _jwt;

        public AuthService(UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> rolemanager, IOptions<JWT> jwt)
        {
            _userManager = usermanager;
            _rolemanager=rolemanager;
            _jwt = jwt.Value;
        }



        public async Task<AuthDto> RegisterAsync(RegisterDto model)
        {
            if( await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthDto { Message="Email is already registered"};

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthDto { Message = "Username is already registered!" };

            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,

            };

            var result = await _userManager.CreateAsync(user,model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthDto { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user); 

            return new AuthDto
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        public async Task<AuthDto> LoginAsync(LoginDto model)
        {
            var authDto = new AuthDto();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authDto.Message = "Email or Password is incorrect!";
                return authDto;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authDto.IsAuthenticated = true;
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDto.Email = user.Email;
            authDto.Username = user.UserName;
            authDto.ExpiresOn = jwtSecurityToken.ValidTo;
            authDto.Roles = rolesList.ToList();

            return authDto;
        }

        public async Task<string> AddRoleAsync(RoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
                return "Invalid USER ID";
            if(! await _rolemanager.RoleExistsAsync(model.Role))
                return "Invalid Role";
            if (await _userManager.IsInRoleAsync(user,model.Role))
                return "Invalid Role";
            var result = await _userManager.AddToRoleAsync(user,model.Role);
            if (!result.Succeeded)
                return "Failed, Try Again";
            else
                return "";



        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


    }
}
