using FinancialBillingService.Model;

namespace FinancialBillingService.Interfaces
{
    public interface IServiceOrderRepository
    {
        Task<ServiceOrder?> GetByIdAsync(int id);
        Task<IEnumerable<ServiceOrder>> GetByVisitIdAsync(int visitId);
        Task<ServiceOrder> CreateAsync(ServiceOrder serviceOrder);
        Task<ServiceOrder> UpdateAsync(ServiceOrder serviceOrder);
        Task<bool> DeleteAsync(int id);
    }
}
