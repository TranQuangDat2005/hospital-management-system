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
            // BR22: Kiểm tra khung giờ phải ở tương lai
            if (dto.AppointmentDate <= DateTime.UtcNow)
                throw new ArgumentException("Lịch hẹn phải được đặt cho thời điểm trong tương lai.");

            // BR15, BR23: Không trùng lặp khung giờ cho cùng một bệnh nhân
            if (await _appointmentRepository.HasAppointmentAtTimeAsync(patientId, dto.AppointmentDate))
                throw new ArgumentException("Bạn đã có lịch hẹn vào khung giờ này, vui lòng chọn thời gian khác.");

            var newAppointment = new Appointments
            {
                PatientID = patientId,
                DoctorID = dto.DoctorID,
                // Giả định DepartmentID lấy từ Doctor, hoặc truyền từ frontend. Tạm hardcode là 1.
                DepartmentID = 1,
                AppointmentDate = dto.AppointmentDate,
                SymptomsDescription = dto.Symptoms,
                // Trạng thái mặc định
                Status = "Pending"
            };

            return await _appointmentRepository.CreateAsync(newAppointment);
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Không tìm thấy lịch hẹn.");

            // BR22: Giờ mới phải ở tương lai
            if (newDate <= DateTime.UtcNow)
                throw new ArgumentException("Lịch thay đổi phải nằm ở tương lai.");

            // BR15, BR23: Không trùng lặp
            if (await _appointmentRepository.HasAppointmentAtTimeAsync(appointment.PatientID, newDate))
                throw new ArgumentException("Bệnh nhân đã có lịch khác vào thời gian này.");

            appointment.AppointmentDate = newDate;
            appointment.Status = "Rescheduled"; // Thay đổi trạng thái thay vì tạo mới

            await _appointmentRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Không tìm thấy lịch hẹn.");

            // Chuyển trạng thái thay vì Hard / Soft Delete để lưu Log
            appointment.Status = "Cancelled";
            await _appointmentRepository.UpdateAsync(appointment);
            return true;
        }
    }
}
