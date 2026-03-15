using Microsoft.EntityFrameworkCore;
using FinancialBillingService.Data;
using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;

namespace FinancialBillingService.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly FinancialBillingDbContext _context;

        public ServiceRepository(FinancialBillingDbContext context)
        {
            _context = context;
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceID == id);
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetAllActiveServicesAsync()
        {
            return await _context.Services.Where(s => s.Status == "Active").ToListAsync();
        }

        public async Task<Service> CreateAsync(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<Service> UpdateAsync(Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
