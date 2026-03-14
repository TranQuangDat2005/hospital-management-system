using Microsoft.EntityFrameworkCore;
using User_Authentication_Service.Data;
using User_Authentication_Service.Interfaces;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserAuthenDbContext _context;

        public UserRepository(UserAuthenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<Users?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserID == userId);
        }

        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Users>> GetUsersByDepartmentAsync(int departmentId)
        {
            return await _context.Users.AsNoTracking()
                .Where(u => u.DepartmentID == departmentId)
                .ToListAsync();
        }

        public async Task AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public void UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
                _context.Users.Remove(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

