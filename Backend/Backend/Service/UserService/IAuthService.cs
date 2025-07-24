using Backend.DTO;
using Backend.Model;

namespace Backend.Service.UserService
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);
        Task<bool> RegisterAsync(RegisterUserDto dto);
        Task<User?> GetByUsernameAsync(string username);
    }
}
