using FinancialBillingService.Model;

namespace FinancialBillingService.Interfaces
{
    public interface IHealthInsuranceRepository
    {
        Task<HealthInsurance?> GetByIdAsync(int id);
        Task<HealthInsurance?> GetByPatientIdAsync(int patientId);
        Task<HealthInsurance> CreateAsync(HealthInsurance healthInsurance);
        Task<HealthInsurance> UpdateAsync(HealthInsurance healthInsurance);
        Task<bool> DeleteAsync(int id);
    }
}
