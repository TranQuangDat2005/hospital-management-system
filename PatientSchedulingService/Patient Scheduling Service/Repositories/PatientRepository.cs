using Microsoft.EntityFrameworkCore;
using Patient_Scheduling_Service.Data;
using Patient_Scheduling_Service.Interfaces;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientScheduleDbContext _context;

        public PatientRepository(PatientScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<Patients?> GetByIdAsync(int id)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == id && !p.IsDeleted);
        }

        public async Task<Patients?> GetByCccdOrPhoneAsync(string? cccd, string? phone)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => 
                    !p.IsDeleted &&
                    ((!string.IsNullOrEmpty(cccd) && p.IdentityCard == cccd) || 
                    (!string.IsNullOrEmpty(phone) && p.Phone == phone)));
        }

        public async Task<bool> IsCccdExistsAsync(string cccd)
        {
            return await _context.Patients.AnyAsync(p => p.IdentityCard == cccd && !p.IsDeleted);
        }

        public async Task<Patients> CreateAsync(Patients patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patients> UpdateAsync(Patients patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null || patient.IsDeleted) return false;

            patient.IsDeleted = true;
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
