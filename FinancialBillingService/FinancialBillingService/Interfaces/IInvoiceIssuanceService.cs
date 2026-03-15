using System.Threading.Tasks;

namespace FinancialBillingService.Interfaces
{
    public interface IInvoiceIssuanceService
    {
        Task<byte[]> GenerateInvoicePdfAsync(int invoiceId);
    }
}
