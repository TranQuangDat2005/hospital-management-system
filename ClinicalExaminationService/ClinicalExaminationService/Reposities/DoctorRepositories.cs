using ClinicalExaminationService.Data;
using ClinicalExaminationService.Interfaces;
using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicalExaminationService.Reposities
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly ClinicalExamDbContext _context;
        public PrescriptionRepository(ClinicalExamDbContext context) => _context = context;

        public async Task<Prescription?> GetByVisitIdAsync(Guid visitId) =>
            await _context.Prescriptions
                .Include(p => p.PrescriptionItems)
                .FirstOrDefaultAsync(p => p.VisitId == visitId);

        public async Task<Prescription?> GetByIdAsync(Guid id) =>
            await _context.Prescriptions
                .Include(p => p.PrescriptionItems)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Prescription> AddAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task<Prescription> UpdateAsync(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }
    }

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly ClinicalExamDbContext _context;
        public ServiceOrderRepository(ClinicalExamDbContext context) => _context = context;

        public async Task<IEnumerable<ServiceOrder>> GetByVisitIdAsync(Guid visitId) =>
            await _context.ServiceOrders.Where(o => o.VisitId == visitId).ToListAsync();

        public async Task<ServiceOrder> AddAsync(ServiceOrder order)
        {
            _context.ServiceOrders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }

    public class FollowUpAppointmentRepository : IFollowUpAppointmentRepository
    {
        private readonly ClinicalExamDbContext _context;
        public FollowUpAppointmentRepository(ClinicalExamDbContext context) => _context = context;

        public async Task<FollowUpAppointment> AddAsync(FollowUpAppointment followUp)
        {
            _context.FollowUpAppointments.Add(followUp);
            await _context.SaveChangesAsync();
            return followUp;
        }

        public async Task<IEnumerable<FollowUpAppointment>> GetByPatientIdAsync(Guid patientId) =>
            await _context.FollowUpAppointments
                .Where(f => f.PatientId == patientId)
                .OrderByDescending(f => f.FollowUpDate)
                .ToListAsync();
    }
}
