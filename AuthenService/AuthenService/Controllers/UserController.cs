using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Authentication_Service.DTOs;
using User_Authentication_Service.Interfaces;

namespace User_Authentication_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("doctors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _userService.GetDoctorsAsync();
            return Ok(doctors);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"Không tìm thấy người dùng ID={id}." });

            return Ok(user);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, user) = await _userService.CreateUserAsync(dto);
            if (!success)
                return Conflict(new { message });

            return CreatedAtAction(nameof(GetById), new { id = user!.UserID }, new
            {
                message,
                data = user
            });
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(currentUserIdStr, out int currentUserId) && currentUserId == id)
            {
                if (!string.IsNullOrEmpty(dto.Role) && dto.Role != "Admin")
                    return BadRequest(new { message = "BR10: Quản trị viên không thể tự hạ quyền của chính mình." });
                if (!string.IsNullOrEmpty(dto.Status) && dto.Status == "Inactive")
                    return BadRequest(new { message = "BR10: Quản trị viên không thể tự khóa tài khoản của chính mình." });
            }

            var (success, message, user) = await _userService.UpdateUserAsync(id, dto);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message, data = user });
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(currentUserIdStr, out int currentUserId) && currentUserId == id)
            {
                return BadRequest(new { message = "BR10: Quản trị viên không thể tự xóa tài khoản của chính mình." });
            }

            var (success, message) = await _userService.DeleteUserAsync(id);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message });
        }
    }
}
