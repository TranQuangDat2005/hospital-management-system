namespace FinancialBillingService.DTOs
{
    public class PaymentRequestDto
    {
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty; 
        public string PaymentMethod { get; set; } = string.Empty; 
    }

    public class PaymentResponseDto
    {
        public int InvoiceID { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PaymentTime { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class PaymentInfoDto
    {
        public int VisitID { get; set; }
        public decimal TotalAmountToPay { get; set; }
        public string QrCodeData { get; set; } = string.Empty; 
        public List<ServiceOrderItemDto> Services { get; set; } = new();
    }

    public class VnPaymentRequestDto
    {
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public decimal Amount { get; set; } 
    }

    public class VnPaymentResponseDto
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string OrderDescription { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string VnPayResponseCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
    public class PaymentHistoryResponseDto
    {
        public int InvoiceID { get; set; }
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal InsurancePaid { get; set; }
        public decimal PatientPaid { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
        public string PdfUrl { get; set; } = string.Empty; 
    }
}
