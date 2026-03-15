using Microsoft.EntityFrameworkCore;
using Patient_Scheduling_Service.Data;
using Patient_Scheduling_Service.Interfaces;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PatientScheduleDbContext _context;

        public AppointmentRepository(PatientScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<Appointments?> GetByIdAsync(int id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentID == id && !a.IsDeleted);
        }

        public async Task<IEnumerable<Appointments>> GetPatientAppointmentsAsync(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientID == patientId && !a.IsDeleted)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<bool> HasAppointmentAtTimeAsync(int patientId, DateTime appointmentDate)
        {
            // Kiểm tra xem bệnh nhân đã có một cuộc hẹn trong cùng một khung giờ hay không.
            // Giả định một lịch hẹn trùng khớp khi nó diễn ra đúng tại thời gian AppointmentDate đó.
            return await _context.Appointments
                .AnyAsync(a => a.PatientID == patientId && a.AppointmentDate == appointmentDate && !a.IsDeleted);
        }

        public async Task<Appointments> CreateAsync(Appointments appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointments> UpdateAsync(Appointments appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null || appointment.IsDeleted) return false;

            appointment.IsDeleted = true;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
