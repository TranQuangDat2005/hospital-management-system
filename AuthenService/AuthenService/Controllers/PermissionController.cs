using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Authentication_Service.DTOs;
using User_Authentication_Service.Interfaces;

namespace User_Authentication_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Lấy danh sách tất cả quyền. Yêu cầu: permissions.read
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var result = await _permissionService.GetAllPermissionsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Tạo quyền mới. Yêu cầu: permissions.create
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, data) = await _permissionService.CreatePermissionAsync(dto);
            if (!success)
                return Conflict(new { message });

            return CreatedAtAction(nameof(GetAllPermissions), new { }, new { message, data });
        }

        /// <summary>
        /// Xóa quyền theo ID. Yêu cầu: permissions.delete
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var (success, message) = await _permissionService.DeletePermissionAsync(id);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message });
        }

        /// <summary>
        /// Lấy danh sách tất cả Role cùng quyền của chúng. Yêu cầu: permissions.read
        /// </summary>
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _permissionService.GetAllRolesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách quyền của một Role cụ thể. Yêu cầu: permissions.read
        /// </summary>
        [HttpGet("roles/{roleName}")]
        public async Task<IActionResult> GetRolePermissions(string roleName)
        {
            var result = await _permissionService.GetRolePermissionsAsync(roleName);
            if (result == null)
                return NotFound(new { message = $"Không tìm thấy role '{roleName}' hoặc role chưa có quyền nào." });

            return Ok(result);
        }

        /// <summary>
        /// Gán quyền cho Role. Yêu cầu: permissions.assign
        /// </summary>
        [HttpPost("roles/{roleName}/permissions")]
        public async Task<IActionResult> AssignPermission(string roleName, [FromBody] AssignPermissionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, data) = await _permissionService.AssignPermissionAsync(roleName, dto);
            if (!success)
                return Conflict(new { message });

            return CreatedAtAction(nameof(GetRolePermissions), new { roleName }, new { message, data });
        }

        /// <summary>
        /// Thu hồi quyền khỏi Role theo RolePermissionID. Yêu cầu: permissions.assign
        /// </summary>
        [HttpDelete("roles/permissions/{rolePermissionId:int}")]
        public async Task<IActionResult> RevokePermission(int rolePermissionId)
        {
            var (success, message) = await _permissionService.RevokePermissionAsync(rolePermissionId);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message });
        }
    }
}
