using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Interfaces;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Appointments> BookAppointmentAsync(int patientId, AppointmentBookingDto dto)
        {
            if (dto.AppointmentDate <= DateTime.UtcNow)
                throw new ArgumentException("Lịch hẹn phải được đặt cho thời điểm trong tương lai.");
            if (await _appointmentRepository.HasAppointmentAtTimeAsync(patientId, dto.AppointmentDate))
                throw new ArgumentException("Bạn đã có lịch hẹn vào khung giờ này, vui lòng chọn thời gian khác.");

            var newAppointment = new Appointments
            {
                PatientID = patientId,
                DoctorID = dto.DoctorID,
                DepartmentID = 1,
                AppointmentDate = dto.AppointmentDate,
                SymptomsDescription = dto.Symptoms,
                Status = "Pending"
            };

            return await _appointmentRepository.CreateAsync(newAppointment);
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Không tìm thấy lịch hẹn.");
            if (newDate <= DateTime.UtcNow)
                throw new ArgumentException("Lịch thay đổi phải nằm ở tương lai.");
            if (await _appointmentRepository.HasAppointmentAtTimeAsync(appointment.PatientID, newDate))
                throw new ArgumentException("Bệnh nhân đã có lịch khác vào thời gian này.");

            appointment.AppointmentDate = newDate;
            appointment.Status = "Rescheduled"; 

            await _appointmentRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Không tìm thấy lịch hẹn.");
            appointment.Status = "Cancelled";
            await _appointmentRepository.UpdateAsync(appointment);
            return true;
        }
    }
}
