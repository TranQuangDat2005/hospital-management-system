using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemOperationsService.Data;
using SystemOperationsService.Model;

namespace SystemOperationsService.Repositories
{
    public interface IBugReportRepository
    {
        Task<IEnumerable<BugReport>> GetAllAsync(BugStatus? status = null, BugPriority? priority = null);
        Task<BugReport?> GetByIdAsync(Guid id);
        Task<BugReport> CreateAsync(BugReport report);
        Task UpdateAsync(BugReport report);
    }

    public class BugReportRepository : IBugReportRepository
    {
        private readonly OpsDbContext _context;

        public BugReportRepository(OpsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BugReport>> GetAllAsync(BugStatus? status = null, BugPriority? priority = null)
        {
            var query = _context.BugReports.AsQueryable();

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);
            
            if (priority.HasValue)
                query = query.Where(b => b.Priority == priority.Value);

            return await query.OrderByDescending(b => b.CreatedAt).ToListAsync();
        }

        public async Task<BugReport?> GetByIdAsync(Guid id)
        {
            return await _context.BugReports.FindAsync(id);
        }

        public async Task<BugReport> CreateAsync(BugReport report)
        {
            _context.BugReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task UpdateAsync(BugReport report)
        {
            report.UpdatedAt = DateTime.UtcNow;
            _context.BugReports.Update(report);
            await _context.SaveChangesAsync();
        }
    }
}
