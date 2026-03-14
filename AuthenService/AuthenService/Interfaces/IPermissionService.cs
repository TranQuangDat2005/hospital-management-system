using User_Authentication_Service.DTOs;

namespace User_Authentication_Service.Interfaces
{
    public interface IPermissionService
    {
        // ── Permissions ───────────────────────────────────────────────────────
        Task<IEnumerable<PermissionResponseDto>> GetAllPermissionsAsync();
        Task<(bool Success, string Message, PermissionResponseDto? Data)> CreatePermissionAsync(CreatePermissionDto dto);
        Task<(bool Success, string Message)> DeletePermissionAsync(int id);

        // ── Roles ─────────────────────────────────────────────────────────────
        Task<IEnumerable<RoleSummaryDto>> GetAllRolesAsync();
        Task<RoleSummaryDto?> GetRolePermissionsAsync(string roleName);
        Task<(bool Success, string Message, RolePermissionResponseDto? Data)> AssignPermissionAsync(string roleName, AssignPermissionDto dto);
        Task<(bool Success, string Message)> RevokePermissionAsync(int rolePermissionId);
    }
}
