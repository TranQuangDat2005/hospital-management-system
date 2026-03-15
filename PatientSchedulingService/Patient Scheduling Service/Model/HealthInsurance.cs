namespace Patient_Scheduling_Service.Model
{
    public class HealthInsurance
    {
        public int InsuranceID { get; set; }
        public int PatientID { get; set; }

        public string CardNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int CoverageRate { get; set; }
        public string RegisteredHospital { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;

        public Patients? Patient { get; set; }
    }
}
