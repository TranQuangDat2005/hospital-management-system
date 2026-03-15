using FinancialBillingService.DTOs;

namespace FinancialBillingService.Interfaces
{
    public interface IPaymentProcessingService
    {
        Task<PaymentInfoDto> GetPaymentInfoForScreenAsync(int visitId, int patientId);
        Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request, int cashierId);
        Task<string> CreateVnPaymentUrlAsync(VnPaymentRequestDto request, HttpContext httpContext);
        Task<VnPaymentResponseDto> ExecuteVnPaymentCallbackAsync(IQueryCollection collections);
    }
}
