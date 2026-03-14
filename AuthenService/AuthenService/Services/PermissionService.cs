using User_Authentication_Service.DTOs;
using User_Authentication_Service.Interfaces;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepo;

        public PermissionService(IPermissionRepository permissionRepo)
        {
            _permissionRepo = permissionRepo;
        }

        // ── Permissions ───────────────────────────────────────────────────────

        public async Task<IEnumerable<PermissionResponseDto>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionRepo.GetAllPermissionsAsync();
            return permissions.Select(p => new PermissionResponseDto
            {
                PermissionID  = p.PermissionID,
                Name          = p.Name,
                Description   = p.Description
            });
        }

        public async Task<(bool Success, string Message, PermissionResponseDto? Data)> CreatePermissionAsync(CreatePermissionDto dto)
        {
            if (await _permissionRepo.PermissionNameExistsAsync(dto.Name))
                return (false, $"Quyền '{dto.Name}' đã tồn tại.", null);

            var permission = new Permission
            {
                Name        = dto.Name.Trim(),
                Description = dto.Description.Trim()
            };

            var created = await _permissionRepo.CreatePermissionAsync(permission);

            var result = new PermissionResponseDto
            {
                PermissionID  = created.PermissionID,
                Name          = created.Name,
                Description   = created.Description
            };

            return (true, "Tạo quyền thành công.", result);
        }

        public async Task<(bool Success, string Message)> DeletePermissionAsync(int id)
        {
            var permission = await _permissionRepo.GetPermissionByIdAsync(id);
            if (permission == null)
                return (false, $"Không tìm thấy quyền ID={id}.");

            var deleted = await _permissionRepo.DeletePermissionAsync(id);
            return deleted
                ? (true,  $"Đã xóa quyền '{permission.Name}'.")
                : (false, "Xóa quyền thất bại.");
        }

        // ── Roles ─────────────────────────────────────────────────────────────

        public async Task<IEnumerable<RoleSummaryDto>> GetAllRolesAsync()
        {
            var roleNames = await _permissionRepo.GetAllRoleNamesAsync();
            var result    = new List<RoleSummaryDto>();

            foreach (var roleName in roleNames)
            {
                var rolePerms = await _permissionRepo.GetRolePermissionsAsync(roleName);
                result.Add(new RoleSummaryDto
                {
                    RoleName    = roleName,
                    Permissions = rolePerms.Select(rp => new PermissionResponseDto
                    {
                        PermissionID  = rp.Permission.PermissionID,
                        Name          = rp.Permission.Name,
                        Description   = rp.Permission.Description
                    }).ToList()
                });
            }

            return result;
        }

        public async Task<RoleSummaryDto?> GetRolePermissionsAsync(string roleName)
        {
            var rolePerms = await _permissionRepo.GetRolePermissionsAsync(roleName);
            if (!rolePerms.Any()) return null;

            return new RoleSummaryDto
            {
                RoleName    = roleName,
                Permissions = rolePerms.Select(rp => new PermissionResponseDto
                {
                    PermissionID  = rp.Permission.PermissionID,
                    Name          = rp.Permission.Name,
                    Description   = rp.Permission.Description
                }).ToList()
            };
        }

        public async Task<(bool Success, string Message, RolePermissionResponseDto? Data)> AssignPermissionAsync(string roleName, AssignPermissionDto dto)
        {
            var permission = await _permissionRepo.GetPermissionByIdAsync(dto.PermissionID);
            if (permission == null)
                return (false, $"Không tìm thấy quyền ID={dto.PermissionID}.", null);

            if (await _permissionRepo.RolePermissionExistsAsync(roleName, dto.PermissionID))
                return (false, $"Role '{roleName}' đã có quyền '{permission.Name}'.", null);

            var rp = new RolePermission
            {
                RoleName     = roleName,
                PermissionID = dto.PermissionID
            };

            var created = await _permissionRepo.AssignPermissionToRoleAsync(rp);

            var result = new RolePermissionResponseDto
            {
                RolePermissionID      = created.RolePermissionID,
                RoleName              = created.RoleName,
                PermissionID          = created.PermissionID,
                PermissionName        = created.Permission.Name,
                PermissionDescription = created.Permission.Description
            };

            return (true, $"Đã gán quyền '{permission.Name}' cho role '{roleName}'.", result);
        }

        public async Task<(bool Success, string Message)> RevokePermissionAsync(int rolePermissionId)
        {
            var revoked = await _permissionRepo.RevokePermissionFromRoleAsync(rolePermissionId);
            return revoked
                ? (true,  "Đã thu hồi quyền thành công.")
                : (false, $"Không tìm thấy RolePermission ID={rolePermissionId}.");
        }
    }
}
