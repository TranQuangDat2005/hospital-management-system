using ClinicalExaminationService.DTOs;
using ClinicalExaminationService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicalExaminationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentScheduleRepository _repo;

        public AssignmentController(IAssignmentScheduleRepository repo)
        {
            _repo = repo;
        }

        
        [HttpGet("me")]
        [Authorize(Roles = "Nurse")]
        public async Task<IActionResult> GetMyAssignments([FromQuery] DateTime? date)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var nurseId))
            {
                return Unauthorized("Invalid user token or missing UserId claim.");
            }

            var assignments = await _repo.GetByNurseIdAsync(nurseId, date);

            var response = assignments.Select(a => new AssignmentScheduleResponseDto
            {
                Id = a.Id,
                NurseId = a.NurseId,
                DoctorId = a.DoctorId,
                ClinicRoom = a.ClinicRoom,
                ShiftDate = a.ShiftDate,
                ShiftType = a.ShiftType.ToString()
            });

            return Ok(response);
        }
    }
}

