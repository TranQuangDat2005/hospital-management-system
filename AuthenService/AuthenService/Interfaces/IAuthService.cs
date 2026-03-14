using User_Authentication_Service.DTOs;

namespace User_Authentication_Service.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
        Task<(bool Success, string Message, UserResponseDto? User)> RegisterAsync(RegisterRequestDto dto);
        bool Logout(string token);
        bool IsTokenBlacklisted(string token);
    }
}
