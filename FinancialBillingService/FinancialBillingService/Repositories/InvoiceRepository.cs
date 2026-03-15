using Microsoft.EntityFrameworkCore;
using FinancialBillingService.Data;
using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;

namespace FinancialBillingService.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly FinancialBillingDbContext _context;

        public InvoiceRepository(FinancialBillingDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _context.Invoice.FirstOrDefaultAsync(i => i.InvoiceID == id);
        }

        public async Task<Invoice?> GetByVisitIdAsync(int visitId)
        {
            return await _context.Invoice.FirstOrDefaultAsync(i => i.VisitID == visitId);
        }
        public async Task<IEnumerable<Invoice>> SearchAsync(int? patientId, DateTime? dateFrom, DateTime? dateTo, int? invoiceId, string? paymentMethod)
        {
            var query = _context.Invoice.AsQueryable();

            if (patientId.HasValue && patientId.Value > 0)
                query = query.Where(i => i.PatientID == patientId.Value);

            if (invoiceId.HasValue && invoiceId.Value > 0)
                query = query.Where(i => i.InvoiceID == invoiceId.Value);

            if (dateFrom.HasValue)
                query = query.Where(i => i.IssuedAt >= dateFrom.Value.Date);

            if (dateTo.HasValue)
                query = query.Where(i => i.IssuedAt < dateTo.Value.Date.AddDays(1));

            if (!string.IsNullOrWhiteSpace(paymentMethod))
                query = query.Where(i => i.PaymentMethod.ToLower() == paymentMethod.ToLower());

            return await query.OrderByDescending(i => i.IssuedAt).ToListAsync();
        }

        public async Task<Invoice> CreateAsync(Invoice invoice)
        {
            _context.Invoice.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> UpdateAsync(Invoice invoice)
        {
            _context.Invoice.Update(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null) return false;

            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
