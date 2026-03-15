using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Interfaces;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InsuranceController : ControllerBase
    {
        private readonly IHealthInsuranceRepository _insuranceRepo;

        public InsuranceController(IHealthInsuranceRepository insuranceRepo)
        {
            _insuranceRepo = insuranceRepo;
        }

        
        
        
        
        [HttpPost("verify")]
        [Authorize(Roles = "Receptionist,Admin")]
        public async Task<IActionResult> VerifyAndSave([FromBody] InsuranceVerificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.ExpiryDate < DateTime.UtcNow)
            {
                return BadRequest(new
                {
                    message = "Card is expired. The patient must pay in full.",
                    isExpired = true,
                    coverageRate = 0
                });
            }

            var insurance = new HealthInsurance
            {
                PatientID = dto.PatientID,
                CardNumber = dto.CardNumber,
                ExpiryDate = dto.ExpiryDate,
                CoverageRate = dto.CoverageRate,
                RegisteredHospital = dto.RegisteredHospital,
                IsVerified = true
            };

            var saved = await _insuranceRepo.UpsertAsync(insurance);

            var response = new InsuranceResponseDto
            {
                InsuranceID = saved.InsuranceID,
                PatientID = saved.PatientID,
                CardNumber = saved.CardNumber,
                ExpiryDate = saved.ExpiryDate,
                CoverageRate = saved.CoverageRate,
                RegisteredHospital = saved.RegisteredHospital,
                IsVerified = saved.IsVerified
            };

            return Ok(new { message = "Insurance verified and saved successfully.", data = response });
        }

        
        
        
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Receptionist,Admin,Cashier")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var insurance = await _insuranceRepo.GetByPatientIdAsync(patientId);
            if (insurance == null)
                return NotFound(new { message = "No insurance record found for this patient." });

            var response = new InsuranceResponseDto
            {
                InsuranceID = insurance.InsuranceID,
                PatientID = insurance.PatientID,
                CardNumber = insurance.CardNumber,
                ExpiryDate = insurance.ExpiryDate,
                CoverageRate = insurance.CoverageRate,
                RegisteredHospital = insurance.RegisteredHospital,
                IsVerified = insurance.IsVerified
            };

            return Ok(response);
        }
    }
}

