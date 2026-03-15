using Microsoft.AspNetCore.Mvc;
using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Interfaces;
using System.Security.Claims;

namespace Patient_Scheduling_Service.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        
        [HttpPost("register")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous] 
        public async Task<IActionResult> Register([FromBody] PatientRegistrationDto dto)
        {
            try
            {
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (int.TryParse(userIdClaim, out int loggedInUserId))
                    {
                        dto.UserID = loggedInUserId;
                    }
                }

                var result = await _patientService.RegisterPatientAsync(dto);
                return Ok(new { message = "Hồ sơ bệnh nhân đã được tạo thành công.", patientId = result.PatientID });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        [HttpGet("identify")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Receptionist,Admin")] 
        public async Task<IActionResult> Identify([FromQuery] string? cccd, [FromQuery] string? phone)
        {
            try
            {
                var patient = await _patientService.IdentifyPatientAsync(cccd, phone);
                if (patient == null)
                {
                    return NotFound(new { message = "Không tìm thấy hồ sơ. Đang chuyển hướng sang quy trình tạo hồ sơ mới (Creating State)." });
                }

                return Ok(new { message = "Định danh thành công. (State -> Registered/Identified)", patient });
            }
            catch(ArgumentException ex)
            {
               return BadRequest(new { message = ex.Message });
            }
        }
    }
}
