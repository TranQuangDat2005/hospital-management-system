namespace FinancialBillingService.Model
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int CashierID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal InsurancePaid { get; set; }
        public decimal PatientPaid { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }

        public Invoice()
        {
        }
    }
}
