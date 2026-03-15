using ClinicalExaminationService.DTOs;
using ClinicalExaminationService.Interfaces;
using ClinicalExaminationService.Model;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalExaminationService.Controllers
{
    [ApiController]
    [Route("api/visit")]
    public class VisitRecordController : ControllerBase
    {
        private readonly IExaminationService _examinationService;

        public VisitRecordController(IExaminationService examinationService)
        {
            _examinationService = examinationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? status)
        {
            VisitStatus? parsedStatus = status.HasValue ? (VisitStatus)status.Value : null;
            var visits = await _examinationService.GetAllVisitsAsync(parsedStatus);
            return Ok(visits);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var visit = await _examinationService.GetVisitAsync(id);
            if (visit == null)
                return NotFound(new { message = $"VisitRecord {id} not found." });

            return Ok(visit);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitAsync([FromBody] CreateVisitRecordDto dto)
        {
            var result = await _examinationService.CreateVisitAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateVisitStatusDto dto)
        {
            try
            {
                var newStatus = (VisitStatus)dto.Status;
                var result = await _examinationService.UpdateStatusAsync(id, newStatus);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPatch("{id:guid}/priority")]
        public async Task<IActionResult> TogglePriority(Guid id)
        {
            try
            {
                var result = await _examinationService.TogglePriorityAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
