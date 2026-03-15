using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SystemOperationsService.DTOs;
using SystemOperationsService.Model;
using SystemOperationsService.Repositories;

namespace SystemOperationsService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BugReportsController : ControllerBase
    {
        private readonly IBugReportRepository _repository;
        private readonly ILogger<BugReportsController> _logger;

        public BugReportsController(IBugReportRepository repository, ILogger<BugReportsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BugStatus? status, [FromQuery] BugPriority? priority)
        {
            var reports = await _repository.GetAllAsync(status, priority);
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var report = await _repository.GetByIdAsync(id);
            if (report == null)
            {
                return NotFound(new { message = "Bug report not found." });
            }
            return Ok(report);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBugReportDto dto)
        {
            var report = new BugReport
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                ReportedByUserId = dto.ReportedByUserId
            };

            var created = await _repository.CreateAsync(report);
            
            
            _logger.LogInformation("Bug report created: {BugReportId} by user {UserId}", created.Id, created.ReportedByUserId);

            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        [HttpPatch("{id}/status")]
        
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateBugStatusDto dto)
        {
            var report = await _repository.GetByIdAsync(id);
            if (report == null) return NotFound();

            var oldStatus = report.Status;
            report.Status = dto.Status;
            
            if (!string.IsNullOrEmpty(dto.ResolutionNotes))
                report.ResolutionNotes = dto.ResolutionNotes;
                
            if (!string.IsNullOrEmpty(dto.AssignedToUserId))
                report.AssignedToUserId = dto.AssignedToUserId;

            await _repository.UpdateAsync(report);

            
            _logger.LogInformation("Bug report {BugReportId} status changed from {OldStatus} to {NewStatus} by IT Support {ITUserId}", 
                report.Id, oldStatus, report.Status, dto.AssignedToUserId ?? "System");

            return Ok(new { message = "Status updated successfully.", report });
        }
    }
}

