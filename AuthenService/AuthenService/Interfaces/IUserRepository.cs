using User_Authentication_Service.Model;

namespace User_Authentication_Service.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetUsersAsync();

        Task<Users?> GetUserByIdAsync(int userId);

        Task<Users?> GetUserByUsernameAsync(string username);

        Task<Users?> GetUserByEmailAsync(string email);

        Task<IEnumerable<Users>> GetUsersByDepartmentAsync(int departmentId);

        Task AddUserAsync(Users user);

        void UpdateUserAsync(Users user);

        Task DeleteUserAsync(int userId);

        Task<bool> SaveChangesAsync();
    }
}