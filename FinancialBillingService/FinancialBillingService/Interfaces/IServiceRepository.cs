using FinancialBillingService.Model;

namespace FinancialBillingService.Interfaces
{
    public interface IServiceRepository
    {
        Task<Service?> GetByIdAsync(int id);
        Task<IEnumerable<Service>> GetAllAsync();
        Task<IEnumerable<Service>> GetAllActiveServicesAsync();

        Task<Service> CreateAsync(Service service);
        Task<Service> UpdateAsync(Service service);
        Task<bool> DeleteAsync(int id);
    }
}
