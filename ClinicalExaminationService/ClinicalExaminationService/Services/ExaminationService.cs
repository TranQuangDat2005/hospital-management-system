using ClinicalExaminationService.DTOs;
using ClinicalExaminationService.Interfaces;
using ClinicalExaminationService.Model;

namespace ClinicalExaminationService.Services
{
    public class ExaminationService : IExaminationService
    {
        private readonly IVisitRecordRepository _visitRepository;
        private readonly IVitalSignRepository _vitalSignRepository;
        private readonly IMedicalExaminationRepository _examinationRepository;
        private readonly IIcd10CatalogRepository _icd10Repository;

        public ExaminationService(
            IVisitRecordRepository visitRepository,
            IVitalSignRepository vitalSignRepository,
            IMedicalExaminationRepository examinationRepository,
            IIcd10CatalogRepository icd10Repository)
        {
            _visitRepository = visitRepository;
            _vitalSignRepository = vitalSignRepository;
            _examinationRepository = examinationRepository;
            _icd10Repository = icd10Repository;
        }

        public async Task<VisitRecordResponseDto> CreateVisitAsync(CreateVisitRecordDto dto)
        {
            var today = DateTime.UtcNow;
            var maxQueue = await _visitRepository.GetMaxQueueNumberAsync(dto.DepartmentId, today);

            var record = new VisitRecord
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                ReceptionistId = dto.ReceptionistId,
                DepartmentId = dto.DepartmentId,
                QueueNumber = maxQueue + 1,
                Reason = dto.Reason,
                Status = VisitStatus.Waiting,
                CheckInTime = today
            };

            await _visitRepository.AddAsync(record);
            return MapToResponseDto(record);
        }

        public async Task<IEnumerable<VisitRecordResponseDto>> GetAllVisitsAsync(VisitStatus? status)
        {
            var visits = await _visitRepository.GetAllAsync(status);
            
            return visits.OrderByDescending(v => v.IsPriority)
                         .ThenBy(v => v.QueueNumber)
                         .Select(MapToResponseDto);
        }

        public async Task<VisitRecordResponseDto> TogglePriorityAsync(Guid id)
        {
            var visit = await _visitRepository.GetByIdAsync(id);
            if (visit == null)
            {
                throw new KeyNotFoundException("Visit record not found.");
            }

            visit.IsPriority = !visit.IsPriority;
            await _visitRepository.UpdateAsync(visit);

            return MapToResponseDto(visit);
        }

        public async Task<VisitRecordResponseDto?> GetVisitAsync(Guid id)
        {
            var visit = await _visitRepository.GetByIdAsync(id);
            return visit == null ? null : MapToResponseDto(visit);
        }

        public async Task<VisitRecordResponseDto> RecordVitalSignsAsync(RecordVitalSignsDto dto)
        {
            var visit = await _visitRepository.GetByIdAsync(dto.VisitId);
            if (visit == null)
            {
                throw new KeyNotFoundException("Visit record not found.");
            }

            if (visit.Status == VisitStatus.Closed)
            {
                throw new UnauthorizedAccessException("Cannot modify a closed visit.");
            }

            var existingVitals = await _vitalSignRepository.GetByVisitIdAsync(dto.VisitId);
            if (existingVitals != null)
            {
                throw new InvalidOperationException("Vital signs already recorded for this visit.");
            }

            var vitals = new VitalSign
            {
                Id = Guid.NewGuid(),
                VisitId = dto.VisitId,
                NurseId = dto.NurseId,
                Pulse = dto.Pulse,
                Temperature = dto.Temperature,
                BloodPressure = dto.BloodPressure,
                Weight = dto.Weight,
                RecordedAt = DateTime.UtcNow
            };

            await _vitalSignRepository.AddAsync(vitals);

            
            if (visit.Status == VisitStatus.Waiting)
            {
                visit.Status = VisitStatus.VitalCheck; 
            }

            await _visitRepository.UpdateAsync(visit);
            
            
            visit = await _visitRepository.GetByIdAsync(visit.Id);
            return MapToResponseDto(visit!);
        }

        public async Task<VisitRecordResponseDto> DiagnoseAsync(DiagnoseRequestDto dto)
        {
            var visit = await _visitRepository.GetByIdAsync(dto.VisitId);
            if (visit == null)
            {
                throw new KeyNotFoundException("Visit record not found.");
            }

            
            if (visit.Status == VisitStatus.Closed)
            {
                throw new UnauthorizedAccessException("Cannot modify a closed visit.");
            }

            
            if (visit.VitalSign == null)
            {
                throw new InvalidOperationException("Vital signs must be recorded before diagnosing (BR3 violation).");
            }

            
            if (dto.Icd10Codes == null || !dto.Icd10Codes.Any())
            {
                throw new ArgumentException("Standard ICD-10 code is required for diagnosis (BR4 violation).");
            }
            if (string.IsNullOrEmpty(dto.PrimaryIcd10Code) || !dto.Icd10Codes.Contains(dto.PrimaryIcd10Code))
            {
                throw new ArgumentException("A valid primary ICD-10 code from the list must be selected.");
            }

            
            var examination = await _examinationRepository.GetByVisitIdAsync(dto.VisitId);
            bool isNew = false;
            if (examination == null)
            {
                examination = new MedicalExamination
                {
                    Id = Guid.NewGuid(),
                    VisitId = dto.VisitId,
                    DoctorId = dto.DoctorId
                };
                isNew = true;
            }

            examination.Symptoms = dto.Symptoms;
            examination.Diagnosis = dto.Diagnosis;
            examination.Notes = dto.Notes;
            examination.ExaminedAt = DateTime.UtcNow;

            
            examination.ExaminationIcd10s.Clear();

            
            foreach (var code in dto.Icd10Codes.Distinct())
            {
                var catalogItem = await _icd10Repository.GetByCodeAsync(code);
                if (catalogItem == null)
                {
                    throw new ArgumentException($"ICD-10 code '{code}' does not exist in the master catalog.");
                }

                examination.ExaminationIcd10s.Add(new ExaminationIcd10
                {
                    Id = Guid.NewGuid(),
                    ExaminationId = examination.Id,
                    Icd10Code = code,
                    IsPrimary = code == dto.PrimaryIcd10Code
                });
            }

            if (isNew)
            {
                await _examinationRepository.AddAsync(examination);
            }
            else
            {
                await _examinationRepository.UpdateAsync(examination);
            }

            
            visit.Status = VisitStatus.PendingPayment;
            await _visitRepository.UpdateAsync(visit);

            visit = await _visitRepository.GetByIdAsync(visit.Id);
            return MapToResponseDto(visit!);
        }

        public async Task<VisitRecordResponseDto> UpdateStatusAsync(Guid id, VisitStatus newStatus)
        {
            var visit = await _visitRepository.GetByIdAsync(id);
            if (visit == null)
            {
                throw new KeyNotFoundException("Visit record not found.");
            }

            
            
            
            if (newStatus == VisitStatus.Examining && visit.VitalSign == null)
            {
                throw new InvalidOperationException("Cannot transition to Examining without vital signs (BR3).");
            }

            
            
            if ((int)newStatus < (int)visit.Status)
            {
                 
                 
            }

            visit.Status = newStatus;
            await _visitRepository.UpdateAsync(visit);

            return MapToResponseDto(visit);
        }

        private VisitRecordResponseDto MapToResponseDto(VisitRecord entity)
        {
            var dto = new VisitRecordResponseDto
            {
                Id = entity.Id,
                PatientId = entity.PatientId,
                ReceptionistId = entity.ReceptionistId,
                DepartmentId = entity.DepartmentId,
                QueueNumber = entity.QueueNumber,
                CheckInTime = entity.CheckInTime,
                Reason = entity.Reason,
                Status = entity.Status.ToString(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            if (entity.VitalSign != null)
            {
                dto.VitalSign = new VitalSignResponseDto
                {
                    Id = entity.VitalSign.Id,
                    NurseId = entity.VitalSign.NurseId,
                    Pulse = entity.VitalSign.Pulse,
                    Temperature = entity.VitalSign.Temperature,
                    BloodPressure = entity.VitalSign.BloodPressure,
                    Weight = entity.VitalSign.Weight,
                    RecordedAt = entity.VitalSign.RecordedAt
                };
            }

            if (entity.MedicalExamination != null)
            {
                dto.MedicalExamination = new MedicalExaminationResponseDto
                {
                    Id = entity.MedicalExamination.Id,
                    DoctorId = entity.MedicalExamination.DoctorId,
                    Symptoms = entity.MedicalExamination.Symptoms,
                    Diagnosis = entity.MedicalExamination.Diagnosis,
                    Notes = entity.MedicalExamination.Notes,
                    ExaminedAt = entity.MedicalExamination.ExaminedAt,
                    Icd10Codes = entity.MedicalExamination.ExaminationIcd10s.Select(e => new Icd10Dto
                    {
                        Code = e.Icd10Code,
                        Description = e.Icd10Catalog?.Description ?? string.Empty,
                        IsPrimary = e.IsPrimary
                    }).ToList()
                };
            }

            return dto;
        }
    }
}

