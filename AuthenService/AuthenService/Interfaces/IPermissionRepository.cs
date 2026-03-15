using User_Authentication_Service.Model;

namespace User_Authentication_Service.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<string>> GetPermissionNamesByRoleAsync(string roleName);
        Task<bool> HasPermissionAsync(string roleName, string permissionName);

        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<Permission?> GetPermissionByIdAsync(int id);
        Task<bool> PermissionNameExistsAsync(string name);

        Task<IEnumerable<RolePermission>> GetRolePermissionsAsync(string roleName);
        Task<IEnumerable<string>> GetAllRoleNamesAsync();
        Task<bool> RolePermissionExistsAsync(string roleName, int permissionId);
        Task<Permission> CreatePermissionAsync(Permission permission);
        Task<bool> DeletePermissionAsync(int id);

        Task<RolePermission> AssignPermissionToRoleAsync(RolePermission rolePermission);
        Task<bool> RevokePermissionFromRoleAsync(int rolePermissionId);
    }
}
