using Microsoft.EntityFrameworkCore;
using FinancialBillingService.Data;
using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;

namespace FinancialBillingService.Repositories
{
    public class HealthInsuranceRepository : IHealthInsuranceRepository
    {
        private readonly FinancialBillingDbContext _context;

        public HealthInsuranceRepository(FinancialBillingDbContext context)
        {
            _context = context;
        }

        public async Task<HealthInsurance?> GetByIdAsync(int id)
        {
            return await _context.HealthInsurance.FirstOrDefaultAsync(h => h.InsuranceID == id);
        }

        public async Task<HealthInsurance?> GetByPatientIdAsync(int patientId)
        {
            return await _context.HealthInsurance.FirstOrDefaultAsync(h => h.PatientID == patientId);
        }

        public async Task<HealthInsurance> CreateAsync(HealthInsurance healthInsurance)
        {
            _context.HealthInsurance.Add(healthInsurance);
            await _context.SaveChangesAsync();
            return healthInsurance;
        }

        public async Task<HealthInsurance> UpdateAsync(HealthInsurance healthInsurance)
        {
            _context.HealthInsurance.Update(healthInsurance);
            await _context.SaveChangesAsync();
            return healthInsurance;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var healthInsurance = await _context.HealthInsurance.FindAsync(id);
            if (healthInsurance == null) return false;

            _context.HealthInsurance.Remove(healthInsurance);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
