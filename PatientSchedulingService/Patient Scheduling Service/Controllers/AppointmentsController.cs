using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Interfaces;
using System.Security.Claims;

namespace Patient_Scheduling_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentsController(IAppointmentService appointmentService, IAppointmentRepository appointmentRepository)
        {
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
        }

        
        
        
        
        [HttpPost("book")]
        [Authorize(Roles = "Patient,Receptionist,Admin")]
        public async Task<IActionResult> Book([FromBody] AppointmentBookingDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim, out int loggedInUserId))
                    return Unauthorized(new { message = "Không xác định được danh tính người dùng từ Token." });

                int patientId = loggedInUserId;
                var result = await _appointmentService.BookAppointmentAsync(patientId, dto);

                return Ok(new
                {
                    message = "Lịch hẹn đã được lưu thành công.",
                    appointmentId = result.AppointmentID,
                    status = result.Status
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        
        
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Doctor,Nurse,Receptionist,Admin,Patient")]
        public async Task<IActionResult> GetPatientAppointments(int patientId)
        {
            var appointments = await _appointmentRepository.GetPatientAppointmentsAsync(patientId);
            var result = appointments.Select(a => new AppointmentResponseDto
            {
                AppointmentID = a.AppointmentID,
                PatientID = a.PatientID,
                DoctorID = a.DoctorID,
                DepartmentID = a.DepartmentID,
                AppointmentDate = a.AppointmentDate,
                SymptomsDescription = a.SymptomsDescription,
                Status = a.Status
            });

            return Ok(result);
        }

        
        
        
        [HttpGet("my-appointments")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int patientId))
                return Unauthorized(new { message = "Không xác định được danh tính người dùng." });

            var appointments = await _appointmentRepository.GetPatientAppointmentsAsync(patientId);
            var result = appointments.Select(a => new AppointmentResponseDto
            {
                AppointmentID = a.AppointmentID,
                PatientID = a.PatientID,
                DoctorID = a.DoctorID,
                DepartmentID = a.DepartmentID,
                AppointmentDate = a.AppointmentDate,
                SymptomsDescription = a.SymptomsDescription,
                Status = a.Status
            });

            return Ok(result);
        }

        
        
        
        [HttpPatch("{id}/reschedule")]
        [Authorize(Roles = "Patient,Receptionist,Admin")]
        public async Task<IActionResult> Reschedule(int id, [FromBody] RescheduleDto dto)
        {
            try
            {
                await _appointmentService.RescheduleAppointmentAsync(id, dto.NewDate);
                return Ok(new { message = "Lịch hẹn đã được đổi thành công." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        
        
        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = "Patient,Receptionist,Admin,Doctor")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _appointmentService.CancelAppointmentAsync(id);
                return Ok(new { message = "Lịch hẹn đã bị hủy." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        
        
        
        [HttpGet("doctor/{doctorId}/queue")]
        [Authorize(Roles = "Doctor,Admin,Nurse")]
        public async Task<IActionResult> GetDoctorQueue(int doctorId)
        {
            var appointments = await _appointmentRepository.GetDoctorAppointmentsAsync(doctorId);
            var result = appointments
                .Where(a => a.Status == "Pending" || a.Status == "Rescheduled")
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentID = a.AppointmentID,
                    PatientID = a.PatientID,
                    DoctorID = a.DoctorID,
                    DepartmentID = a.DepartmentID,
                    AppointmentDate = a.AppointmentDate,
                    SymptomsDescription = a.SymptomsDescription,
                    Status = a.Status
                });

            return Ok(result);
        }

        [HttpGet("public-info")]
        [AllowAnonymous]
        public IActionResult GetPublicInfo()
        {
            return Ok(new { message = "Đang phát triển chức năng xem thông tin công khai." });
        }
    }
}

