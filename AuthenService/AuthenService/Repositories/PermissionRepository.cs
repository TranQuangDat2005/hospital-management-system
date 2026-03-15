using Microsoft.EntityFrameworkCore;
using User_Authentication_Service.Data;
using User_Authentication_Service.Interfaces;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly UserAuthenDbContext _context;

        public PermissionRepository(UserAuthenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetPermissionNamesByRoleAsync(string roleName)
        {
            return await _context.RolePermissions
                .AsNoTracking()
                .Where(rp => rp.RoleName == roleName)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(string roleName, string permissionName)
        {
            return await _context.RolePermissions
                .AsNoTracking()
                .Where(rp => rp.RoleName == roleName)
                .Include(rp => rp.Permission)
                .AnyAsync(rp => rp.Permission.Name == permissionName);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Permission?> GetPermissionByIdAsync(int id)
        {
            return await _context.Permissions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PermissionID == id);
        }

        public async Task<bool> PermissionNameExistsAsync(string name)
        {
            return await _context.Permissions
                .AsNoTracking()
                .AnyAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissionsAsync(string roleName)
        {
            return await _context.RolePermissions
                .AsNoTracking()
                .Where(rp => rp.RoleName == roleName)
                .Include(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllRoleNamesAsync()
        {
            return await _context.RolePermissions
                .AsNoTracking()
                .Select(rp => rp.RoleName)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();
        }

        public async Task<bool> RolePermissionExistsAsync(string roleName, int permissionId)
        {
            return await _context.RolePermissions
                .AsNoTracking()
                .AnyAsync(rp => rp.RoleName == roleName && rp.PermissionID == permissionId);
        }

        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return false;

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RolePermission> AssignPermissionToRoleAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
            await _context.Entry(rolePermission).Reference(rp => rp.Permission).LoadAsync();
            return rolePermission;
        }

        public async Task<bool> RevokePermissionFromRoleAsync(int rolePermissionId)
        {
            var rp = await _context.RolePermissions.FindAsync(rolePermissionId);
            if (rp == null) return false;

            _context.RolePermissions.Remove(rp);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
