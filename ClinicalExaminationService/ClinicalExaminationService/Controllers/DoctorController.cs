using ClinicalExaminationService.DTOs;
using ClinicalExaminationService.Interfaces;
using ClinicalExaminationService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicalExaminationService.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    [Authorize(Roles = "Doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IExaminationService _examinationService;
        private readonly IPrescriptionRepository _prescriptionRepo;
        private readonly IServiceOrderRepository _serviceOrderRepo;
        private readonly IFollowUpAppointmentRepository _followUpRepo;
        private readonly IVisitRecordRepository _visitRepo;
        private readonly IMedicalExaminationRepository _examinationRepo;

        public DoctorController(
            IExaminationService examinationService,
            IPrescriptionRepository prescriptionRepo,
            IServiceOrderRepository serviceOrderRepo,
            IFollowUpAppointmentRepository followUpRepo,
            IVisitRecordRepository visitRepo,
            IMedicalExaminationRepository examinationRepo)
        {
            _examinationService = examinationService;
            _prescriptionRepo = prescriptionRepo;
            _serviceOrderRepo = serviceOrderRepo;
            _followUpRepo = followUpRepo;
            _visitRepo = visitRepo;
            _examinationRepo = examinationRepo;
        }

        private Guid GetDoctorId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }

        
        
        
        [HttpGet("queue")]
        public async Task<IActionResult> GetExaminingQueue()
        {
            var visits = await _examinationService.GetAllVisitsAsync(VisitStatus.VitalCheck);
            return Ok(visits);
        }

        
        
        
        
        [HttpGet("visit/{visitId}/emr")]
        public async Task<IActionResult> ViewEMR(Guid visitId)
        {
            var visit = await _examinationService.GetVisitAsync(visitId);
            if (visit == null)
                return NotFound(new { message = "Visit record not found." });

            return Ok(visit);
        }

        
        
        
        
        [HttpPost("visit/{visitId}/prescription")]
        public async Task<IActionResult> CreatePrescription(Guid visitId, [FromBody] CreatePrescriptionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var visit = await _visitRepo.GetByIdAsync(visitId);
            if (visit == null)
                return NotFound(new { message = "Visit not found." });

            var examination = await _examinationRepo.GetByVisitIdAsync(visitId);
            if (examination == null || !examination.ExaminationIcd10s.Any())
                return BadRequest(new { message = "BR4 Violated: An ICD-10 diagnosis must be recorded before creating a prescription." });

            var existing = await _prescriptionRepo.GetByVisitIdAsync(visitId);
            if (existing != null)
                return Conflict(new { message = "A prescription already exists for this visit. To modify, use the update endpoint." });

            var prescription = new Prescription
            {
                Id = Guid.NewGuid(),
                VisitId = visitId,
                DoctorId = GetDoctorId(),
                Notes = dto.Notes,
                PrescriptionItems = dto.Items.Select(i => new PrescriptionItem
                {
                    Id = Guid.NewGuid(),
                    MedicationName = i.MedicationName,
                    Quantity = i.Quantity,
                    Dosage = i.Dosage,
                    Instructions = i.Instructions
                }).ToList()
            };

            var saved = await _prescriptionRepo.AddAsync(prescription);

            return CreatedAtAction(nameof(GetPrescription), new { visitId }, new
            {
                message = "Prescription saved successfully.",
                prescriptionId = saved.Id,
                itemCount = saved.PrescriptionItems.Count
            });
        }

        
        
        
        [HttpGet("visit/{visitId}/prescription")]
        public async Task<IActionResult> GetPrescription(Guid visitId)
        {
            var prescription = await _prescriptionRepo.GetByVisitIdAsync(visitId);
            if (prescription == null)
                return NotFound(new { message = "No prescription found for this visit." });

            return Ok(prescription);
        }

        
        
        
        
        [HttpPost("visit/{visitId}/service-order")]
        public async Task<IActionResult> OrderService(Guid visitId, [FromBody] CreateServiceOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var visit = await _visitRepo.GetByIdAsync(visitId);
            if (visit == null)
                return NotFound(new { message = "Visit not found." });

            if (visit.Status == VisitStatus.Closed || visit.Status == VisitStatus.PaidPendingMeds)
                return BadRequest(new { message = "Cannot order services for a closed or paid visit." });

            var order = new ServiceOrder
            {
                Id = Guid.NewGuid(),
                VisitId = visitId,
                DoctorId = GetDoctorId(),
                ServiceId = dto.ServiceId,
                ServiceName = dto.ServiceName,
                UnitPriceAtOrder = dto.UnitPriceAtOrder,
                Status = "Ordered"
            };

            var saved = await _serviceOrderRepo.AddAsync(order);
            return Ok(new { message = "Service ordered successfully.", orderId = saved.Id });
        }

        
        
        
        [HttpGet("visit/{visitId}/service-orders")]
        public async Task<IActionResult> GetServiceOrders(Guid visitId)
        {
            var orders = await _serviceOrderRepo.GetByVisitIdAsync(visitId);
            return Ok(orders);
        }

        
        
        
        
        [HttpPost("visit/{visitId}/follow-up")]
        public async Task<IActionResult> ScheduleFollowUp(Guid visitId, [FromBody] CreateFollowUpDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.FollowUpDate <= DateTime.UtcNow)
                return BadRequest(new { message = "Follow-up date must be in the future (BR22)." });

            var visit = await _visitRepo.GetByIdAsync(visitId);
            if (visit == null)
                return NotFound(new { message = "Visit not found." });

            var followUp = new FollowUpAppointment
            {
                Id = Guid.NewGuid(),
                VisitId = visitId,
                DoctorId = GetDoctorId(),
                PatientId = visit.PatientId,
                FollowUpDate = dto.FollowUpDate,
                ClinicalNotes = dto.ClinicalNotes
            };

            var saved = await _followUpRepo.AddAsync(followUp);
            return Ok(new { message = "Follow-up scheduled successfully.", followUpId = saved.Id, followUpDate = saved.FollowUpDate });
        }

        
        
        
        [HttpGet("patient/{patientId}/follow-ups")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientFollowUps(Guid patientId)
        {
            var followUps = await _followUpRepo.GetByPatientIdAsync(patientId);
            return Ok(followUps);
        }
    }
}

