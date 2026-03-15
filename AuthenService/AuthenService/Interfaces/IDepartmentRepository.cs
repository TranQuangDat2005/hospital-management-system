using User_Authentication_Service.Model;

namespace User_Authentication_Service.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Departments>> GetAllAsync(bool includeInactive = true);
        Task<Departments?> GetByIdAsync(int id);
        Task<Departments?> GetByNameAsync(string name);
        Task<bool> HasActiveUsersAsync(int departmentId);
        Task AddAsync(Departments department);
        void Update(Departments department);
        Task SoftDeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
