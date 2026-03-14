using User_Authentication_Service.Model;

namespace User_Authentication_Service.Interfaces
{
    public interface IPermissionRepository
    {
        /// <summary>
        /// Lấy danh sách Permission name theo Role.
        /// </summary>
        Task<IEnumerable<string>> GetPermissionNamesByRoleAsync(string roleName);

        /// <summary>
        /// Kiểm tra role có permission cụ thể không.
        /// </summary>
        Task<bool> HasPermissionAsync(string roleName, string permissionName);
    }
}
