using User_Authentication_Service.DTOs;
using User_Authentication_Service.Helpers;
using User_Authentication_Service.Interfaces;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return users.Select(MapToDto);
        }

        public async Task<IEnumerable<UserResponseDto>> GetDoctorsAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return users.Where(u => u.Role == "Doctor" && u.Status == "Active").Select(MapToDto);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<(bool Success, string Message, UserResponseDto? User)> CreateUserAsync(CreateUserDto dto)
        {
            var existing = await _userRepository.GetUserByUsernameAsync(dto.UserName);
            if (existing != null)
                return (false, $"Tên đăng nhập '{dto.UserName}' đã tồn tại.", null);
            var existingEmail = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingEmail != null)
                return (false, $"Email '{dto.Email}' đã được sử dụng.", null);

            var user = new Users
            {
                UserName = dto.UserName,
                PasswordHash = Md5Helper.HashPassword(dto.Password),
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role,
                DepartmentID = dto.DepartmentID,
                Status = dto.Status
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "Tạo người dùng thành công.", MapToDto(user));
        }

        public async Task<(bool Success, string Message, UserResponseDto? User)> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return (false, $"Không tìm thấy người dùng ID={id}.", null);

            
            
            
            
            if (!string.IsNullOrWhiteSpace(dto.FullName))   user.FullName    = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Email))      user.Email       = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Phone))      user.Phone       = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.Role))       user.Role        = dto.Role;
            if (!string.IsNullOrWhiteSpace(dto.Status))     user.Status      = dto.Status;
            if (dto.DepartmentID.HasValue)                  user.DepartmentID = dto.DepartmentID.Value;
            if (!string.IsNullOrWhiteSpace(dto.Password))   user.PasswordHash = Md5Helper.HashPassword(dto.Password);

            _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "Cập nhật thành công.", MapToDto(user));
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return (false, $"Không tìm thấy người dùng ID={id}.");

            await _userRepository.DeleteUserAsync(id);
            await _userRepository.SaveChangesAsync();

            return (true, "Xóa người dùng thành công.");
        }

        private static UserResponseDto MapToDto(Users user) => new()
        {
            UserID       = user.UserID,
            UserName     = user.UserName,
            FullName     = user.FullName,
            Email        = user.Email,
            Phone        = user.Phone,
            Role         = user.Role,
            DepartmentID = user.DepartmentID,
            Status       = user.Status
        };
    }
}

