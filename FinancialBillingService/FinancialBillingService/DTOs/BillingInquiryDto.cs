namespace FinancialBillingService.DTOs
{
    public class ServiceOrderItemDto
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public string ItemType { get; set; } = "Service"; 
    }

    public class BillingInquiryDto
    {
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientCCCD { get; set; } = string.Empty;
        public List<ServiceOrderItemDto> Services { get; set; } = new();
        public decimal TotalRawFee { get; set; }
    }

    public class InsuranceCalculationDto
    {
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public int CoverageRate { get; set; }
        public decimal TotalRawFee { get; set; }
        public decimal InsuranceCoveredAmount { get; set; }
        public decimal CoPaymentAmount { get; set; }
    }
}
