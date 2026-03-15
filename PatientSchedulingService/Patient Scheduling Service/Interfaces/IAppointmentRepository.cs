using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointments?> GetByIdAsync(int id);
        Task<IEnumerable<Appointments>> GetPatientAppointmentsAsync(int patientId);
        Task<bool> HasAppointmentAtTimeAsync(int patientId, DateTime appointmentDate);
        Task<Appointments> CreateAsync(Appointments appointment);
        Task<Appointments> UpdateAsync(Appointments appointment);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> HardDeleteAsync(int id);
    }
}
