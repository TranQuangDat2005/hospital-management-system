using FinancialBillingService.Model;

namespace FinancialBillingService.Interfaces
{
    public interface IServiceCatalogService
    {
        Task<IEnumerable<Service>> GetAllServicesAsync();
        Task<IEnumerable<Service>> GetActiveServicesAsync();
        Task<Service?> GetServiceByIdAsync(int id);
        Task<Service> CreateServiceAsync(Service service);
        Task<Service> UpdateServiceAsync(int id, Service serviceUpdate);
        Task<bool> DeleteServiceAsync(int id);
    }
}
