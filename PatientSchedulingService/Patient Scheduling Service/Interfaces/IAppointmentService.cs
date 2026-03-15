using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointments> BookAppointmentAsync(int patientId, AppointmentBookingDto dto);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate);
        Task<bool> CancelAppointmentAsync(int appointmentId);
    }
}
