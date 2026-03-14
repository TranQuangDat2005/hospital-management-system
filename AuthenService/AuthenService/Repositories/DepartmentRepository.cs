using Microsoft.EntityFrameworkCore;
using User_Authentication_Service.Data;
using User_Authentication_Service.Interfaces;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly UserAuthenDbContext _context;

        public DepartmentRepository(UserAuthenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Departments>> GetAllAsync(bool includeInactive = true)
        {
            var query = _context.Departments.AsNoTracking()
                                .Include(d => d.Users.Where(u => u.Status == "Active"));

            if (!includeInactive)
                return await query.Where(d => d.Status == "Active").ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<Departments?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Users.Where(u => u.Status == "Active"))
                .FirstOrDefaultAsync(d => d.DepartmentID == id);
        }

        public async Task<Departments?> GetByNameAsync(string name)
        {
            return await _context.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> HasActiveUsersAsync(int departmentId)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.DepartmentID == departmentId && u.Status == "Active");
        }

        public async Task AddAsync(Departments department)
        {
            await _context.Departments.AddAsync(department);
        }

        public void Update(Departments department)
        {
            _context.Departments.Update(department);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept != null)
            {
                dept.IsDeleted = true;
                dept.Status = "Inactive";
                _context.Departments.Update(dept);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
