using Microsoft.EntityFrameworkCore;
using FinancialBillingService.Data;
using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;

namespace FinancialBillingService.Repositories
{
    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly FinancialBillingDbContext _context;

        public ServiceOrderRepository(FinancialBillingDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceOrder?> GetByIdAsync(int id)
        {
            return await _context.ServiceOrders.FirstOrDefaultAsync(s => s.OrderID == id);
        }

        public async Task<IEnumerable<ServiceOrder>> GetByVisitIdAsync(int visitId)
        {
            return await _context.ServiceOrders.Where(s => s.VisitID == visitId).ToListAsync();
        }

        public async Task<ServiceOrder> CreateAsync(ServiceOrder serviceOrder)
        {
            _context.ServiceOrders.Add(serviceOrder);
            await _context.SaveChangesAsync();
            return serviceOrder;
        }

        public async Task<ServiceOrder> UpdateAsync(ServiceOrder serviceOrder)
        {
            _context.ServiceOrders.Update(serviceOrder);
            await _context.SaveChangesAsync();
            return serviceOrder;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var serviceOrder = await _context.ServiceOrders.FindAsync(id);
            if (serviceOrder == null) return false;

            _context.ServiceOrders.Remove(serviceOrder);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
