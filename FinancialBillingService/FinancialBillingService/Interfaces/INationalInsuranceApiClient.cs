namespace FinancialBillingService.Interfaces
{
    public interface INationalInsuranceApiClient
    {
        Task<int> GetCoverageRateAsync(string cardNumber, string patientCccd = null);
    }
}
