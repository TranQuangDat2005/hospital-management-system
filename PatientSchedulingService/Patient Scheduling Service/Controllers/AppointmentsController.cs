using Microsoft.AspNetCore.Mvc;
using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Interfaces;
using System.Security.Claims;

namespace Patient_Scheduling_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        
        [HttpPost("book")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Patient,Receptionist,Admin")] 
        public async Task<IActionResult> Book([FromBody] AppointmentBookingDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim, out int loggedInUserId))
                {
                    return Unauthorized(new { message = "Không xác định được danh tính người dùng từ Token." });
                }
                int patientId = loggedInUserId; 
                
                var result = await _appointmentService.BookAppointmentAsync(patientId, dto);
                return Ok(new { message = "Lịch hẹn đã được lưu thành công.", appointmentId = result.AppointmentID, status = result.Status });
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        [HttpGet("public-info")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous] 
        public IActionResult GetPublicInfo()
        {
            return Ok(new { message = "Đang phát triển chức năng xem thông tin công khai." });
        }
    }
}
