namespace FinancialBillingService.Model
{
    public class HealthInsurance
    {
        public int InsuranceID { get; set; }
        public int PatientID { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CoverageRate { get; set; }

        public HealthInsurance(int insuranceID, int patientID, string cardNumber, DateTime expiryDate, int coverageRate)
        {
            InsuranceID = insuranceID;
            PatientID = patientID;
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            CoverageRate = coverageRate;
        }

        public HealthInsurance()
        {
        }
    }
}
