using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Authentication_Service.DTOs;
using User_Authentication_Service.Interfaces;

namespace User_Authentication_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản đã bị khóa." });

            return Ok(result);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, user) = await _authService.RegisterAsync(dto);

            if (!success)
                return Conflict(new { message });

            return StatusCode(StatusCodes.Status201Created, new { message, data = user });
        }
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.StartsWith("Bearer ") == true
                ? authHeader["Bearer ".Length..]
                : null;

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Không tìm thấy token." });

            _authService.Logout(token);
            return Ok(new { message = "Đăng xuất thành công." });
        }
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe()
        {
            var userName = User.Identity?.Name;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var fullName = User.FindFirst("FullName")?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var departmentId = User.FindFirst("DepartmentID")?.Value;

            return Ok(new
            {
                userName,
                fullName,
                email,
                role,
                departmentId
            });
        }
    }
}
