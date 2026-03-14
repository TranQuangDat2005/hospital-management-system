using User_Authentication_Service.DTOs;
using User_Authentication_Service.Helpers;
using User_Authentication_Service.Interfaces;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(dto.UserName);
            if (user == null)
                return null;

            if (user.Status != "Active")
                return null;

            if (!Md5Helper.VerifyPassword(dto.Password, user.PasswordHash))
                return null;

            var (token, expiresAt) = _jwtHelper.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = new UserResponseDto
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = user.Role,
                    DepartmentID = user.DepartmentID,
                    Status = user.Status
                }
            };
        }

        public async Task<(bool Success, string Message, UserResponseDto? User)> RegisterAsync(RegisterRequestDto dto)
        {
            var existingUsername = await _userRepository.GetUserByUsernameAsync(dto.UserName);
            if (existingUsername != null)
                return (false, $"Tên đăng nhập '{dto.UserName}' đã được sử dụng.", null);

            var existingEmail = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingEmail != null)
                return (false, $"Email '{dto.Email}' đã được đăng ký.", null);

            var user = new Users
            {
                UserName     = dto.UserName.Trim(),
                PasswordHash = Md5Helper.HashPassword(dto.Password),
                FullName     = dto.FullName.Trim(),
                Email        = dto.Email.Trim(),
                Phone        = dto.Phone.Trim(),
                Role         = "Patient",   
                DepartmentID = 0,           
                Status       = "Active"     
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "Đăng ký tài khoản thành công. Bạn có thể đăng nhập ngay bây giờ.", new UserResponseDto
            {
                UserID       = user.UserID,
                UserName     = user.UserName,
                FullName     = user.FullName,
                Email        = user.Email,
                Phone        = user.Phone,
                Role         = user.Role,
                DepartmentID = user.DepartmentID,
                Status       = user.Status
            });
        }

        public bool Logout(string token)
        {
            return _jwtHelper.BlacklistToken(token);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _jwtHelper.IsBlacklisted(token);
        }
    }
}
