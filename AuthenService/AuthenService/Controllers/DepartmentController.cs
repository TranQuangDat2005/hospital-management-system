using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Authentication_Service.DTOs;
using User_Authentication_Service.Interfaces;

namespace User_Authentication_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _deptService;

        public DepartmentController(IDepartmentService deptService)
        {
            _deptService = deptService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = false)
        {
            var depts = await _deptService.GetAllAsync(includeInactive: !activeOnly);
            return Ok(depts);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dept = await _deptService.GetByIdAsync(id);
            if (dept == null)
                return NotFound(new { message = $"Không tìm thấy phòng ban ID={id}." });
            return Ok(dept);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, dept) = await _deptService.CreateAsync(dto);
            if (!success)
                return Conflict(new { message });

            return CreatedAtAction(nameof(GetById), new { id = dept!.DepartmentID }, new
            {
                message,
                data = dept
            });
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, dept) = await _deptService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message, data = dept });
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _deptService.DeleteAsync(id);
            if (!success)
            {
                if (message.Contains("không tìm thấy", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message });
                return Conflict(new { message });
            }

            return Ok(new { message });
        }
    }
}
