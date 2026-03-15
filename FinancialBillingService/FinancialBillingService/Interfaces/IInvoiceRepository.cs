using FinancialBillingService.Model;

namespace FinancialBillingService.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<Invoice?> GetByIdAsync(int id);
        Task<Invoice?> GetByVisitIdAsync(int visitId);
        Task<IEnumerable<Invoice>> SearchAsync(int? patientId, DateTime? dateFrom, DateTime? dateTo, int? invoiceId, string? paymentMethod);
        Task<Invoice> CreateAsync(Invoice invoice);
        Task<Invoice> UpdateAsync(Invoice invoice);
        Task<bool> DeleteAsync(int id);
    }
}

