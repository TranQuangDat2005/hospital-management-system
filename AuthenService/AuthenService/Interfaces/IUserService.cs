using User_Authentication_Service.DTOs;

namespace User_Authentication_Service.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<IEnumerable<UserResponseDto>> GetDoctorsAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<(bool Success, string Message, UserResponseDto? User)> CreateUserAsync(CreateUserDto dto);
        Task<(bool Success, string Message, UserResponseDto? User)> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<(bool Success, string Message)> DeleteUserAsync(int id);
    }
}
