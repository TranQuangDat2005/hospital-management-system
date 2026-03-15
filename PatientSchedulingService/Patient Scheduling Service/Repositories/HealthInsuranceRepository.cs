using Microsoft.EntityFrameworkCore;
using Patient_Scheduling_Service.Data;
using Patient_Scheduling_Service.Interfaces;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Repositories
{
    public class HealthInsuranceRepository : IHealthInsuranceRepository
    {
        private readonly PatientScheduleDbContext _context;

        public HealthInsuranceRepository(PatientScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<HealthInsurance?> GetByPatientIdAsync(int patientId)
        {
            return await _context.HealthInsurances
                .FirstOrDefaultAsync(h => h.PatientID == patientId);
        }

        public async Task<HealthInsurance> UpsertAsync(HealthInsurance insurance)
        {
            var existing = await _context.HealthInsurances
                .FirstOrDefaultAsync(h => h.PatientID == insurance.PatientID);

            if (existing == null)
            {
                _context.HealthInsurances.Add(insurance);
            }
            else
            {
                existing.CardNumber = insurance.CardNumber;
                existing.ExpiryDate = insurance.ExpiryDate;
                existing.CoverageRate = insurance.CoverageRate;
                existing.RegisteredHospital = insurance.RegisteredHospital;
                existing.IsVerified = insurance.IsVerified;
                _context.HealthInsurances.Update(existing);
                insurance = existing;
            }

            await _context.SaveChangesAsync();
            return insurance;
        }
    }
}
