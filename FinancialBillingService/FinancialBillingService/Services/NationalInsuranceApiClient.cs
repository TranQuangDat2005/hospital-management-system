using FinancialBillingService.Interfaces;

namespace FinancialBillingService.Services
{
    public class NationalInsuranceApiClient : INationalInsuranceApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NationalInsuranceApiClient> _logger;

        public NationalInsuranceApiClient(HttpClient httpClient, ILogger<NationalInsuranceApiClient> logger)
        {
            _httpClient = httpClient;
            var baseUrl = Environment.GetEnvironmentVariable("InsuranceApi__BaseUrl");
            if (!string.IsNullOrEmpty(baseUrl))
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
            }
            _logger = logger;
        }

        public async Task<int> GetCoverageRateAsync(string cardNumber, string patientCccd = null)
        {
            _logger.LogInformation($"Mocking call to National Insurance API for card: {cardNumber}");
            if (string.IsNullOrEmpty(cardNumber)) return 0;

            if (cardNumber.StartsWith("CC") || cardNumber.StartsWith("TE")) 
            {
                return 100; 
            }
            if (cardNumber.StartsWith("HT") || cardNumber.StartsWith("HN"))
            {
                return 95; 
            }
            if (cardNumber.StartsWith("GD") || cardNumber.StartsWith("DN"))
            {
                return 80; 
            }
            return 80; 
        }
    }
}
