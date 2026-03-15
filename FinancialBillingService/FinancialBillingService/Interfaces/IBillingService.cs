using FinancialBillingService.DTOs;

namespace FinancialBillingService.Interfaces
{
    public interface IBillingService
    {
        Task<BillingInquiryDto> GetBillingInquiryAsync(int visitId);
        Task<InsuranceCalculationDto> CalculateInsuranceAsync(int visitId, int patientId);
    }
}
