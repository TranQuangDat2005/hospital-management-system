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

        /// <summary>
        /// Lấy tất cả phòng ban. Query param: activeOnly=true chỉ lấy Active (BR11 - routing/billing).
        /// Yêu cầu quyền: departments.read
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = false)
        {
            var depts = await _deptService.GetAllAsync(includeInactive: !activeOnly);
            return Ok(depts);
        }

        /// <summary>
        /// Lấy chi tiết phòng ban theo ID.
        /// Yêu cầu quyền: departments.read
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dept = await _deptService.GetByIdAsync(id);
            if (dept == null)
                return NotFound(new { message = $"Không tìm thấy phòng ban ID={id}." });
            return Ok(dept);
        }

        /// <summary>
        /// Tạo phòng ban mới. Trạng thái mặc định: Inactive.
        /// Yêu cầu quyền: departments.create (Admin only)
        /// </summary>
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

        /// <summary>
        /// Cập nhật phòng ban. Admin có thể đổi Status từ Inactive → Active.
        /// Yêu cầu quyền: departments.update (Admin only)
        /// </summary>
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

        /// <summary>
        /// Xóa phòng ban (soft delete). Sẽ bị chặn nếu còn nhân viên Active (dependency check - BR).
        /// Yêu cầu quyền: departments.delete (Admin only)
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _deptService.DeleteAsync(id);
            if (!success)
            {
                // 409 Conflict khi còn dependency, 404 khi không tìm thấy
                if (message.Contains("không tìm thấy", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message });
                return Conflict(new { message });
            }

            return Ok(new { message });
        }
    }
}
