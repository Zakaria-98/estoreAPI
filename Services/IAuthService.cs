using storeAPI.Dto;

namespace storeAPI.Services
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto model);
        Task<AuthDto> LoginAsync(LoginDto model);

        Task<string> AddRoleAsync(RoleDto model);
    }
}
