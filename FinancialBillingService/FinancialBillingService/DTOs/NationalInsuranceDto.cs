namespace FinancialBillingService.DTOs
{
    public class NationalInsuranceTokenResponseDto
    {
        public int MaKetQua { get; set; }
        public NationalInsuranceApiKey APIKey { get; set; }
    }

    public class NationalInsuranceApiKey
    {
        public string access_token { get; set; }
        public string id_token { get; set; }
        public string token_type { get; set; }
        public string username { get; set; }
        public int expires_in { get; set; }
    }
}
