using Microsoft.EntityFrameworkCore;
using User_Authentication_Service.Data;
using User_Authentication_Service.Interfaces;

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
    }
}
