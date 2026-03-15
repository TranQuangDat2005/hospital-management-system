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
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, user) = await _userService.UpdateUserAsync(id, dto);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message, data = user });
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _userService.DeleteUserAsync(id);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message });
        }
    }
}
