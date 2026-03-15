using ClinicalExaminationService.DTOs;
using ClinicalExaminationService.Interfaces;
using ClinicalExaminationService.Model;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalExaminationService.Controllers
{
    [ApiController]
    [Route("api/examination")]
    public class ExaminationController : ControllerBase
    {
        private readonly IExaminationService _examinationService;
        private readonly IIcd10CatalogRepository _icd10Repo;

        public ExaminationController(IExaminationService examinationService, IIcd10CatalogRepository icd10Repo)
        {
            _examinationService = examinationService;
            _icd10Repo = icd10Repo;
        }

        [HttpPost("vital-signs")]
        public async Task<IActionResult> RecordVitalSigns([FromBody] RecordVitalSignsDto dto)
        {
            try
            {
                var result = await _examinationService.RecordVitalSignsAsync(dto);
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

        [HttpGet("icd10")]
        public async Task<IActionResult> SearchIcd10([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(new { message = "Keyword is required." });

            var results = await _icd10Repo.SearchAsync(keyword);
            return Ok(results.Select(c => new Icd10Dto
            {
                Code = c.Code,
                Description = c.Description
            }));
        }

        [HttpPost("diagnose")]
        public async Task<IActionResult> Diagnose([FromBody] DiagnoseRequestDto dto)
        {
            try
            {
                var result = await _examinationService.DiagnoseAsync(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
