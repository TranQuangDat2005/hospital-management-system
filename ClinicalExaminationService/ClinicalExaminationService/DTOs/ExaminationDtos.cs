namespace ClinicalExaminationService.DTOs
{
    public class CreateVisitRecordDto
    {
        public Guid PatientId { get; set; }
        public Guid? ReceptionistId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? Reason { get; set; }
    }

    public class UpdateVisitStatusDto
    {
        public int Status { get; set; }
    }

    public class RecordVitalSignsDto
    {
        public Guid VisitId { get; set; }
        public Guid? NurseId { get; set; }
        public int Pulse { get; set; }
        public decimal Temperature { get; set; }
        public string BloodPressure { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }

    public class DiagnoseRequestDto
    {
        public Guid VisitId { get; set; }
        public Guid DoctorId { get; set; }
        public string Symptoms { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public List<string> Icd10Codes { get; set; } = new();
        public string PrimaryIcd10Code { get; set; } = string.Empty;
    }

    public class VitalSignResponseDto
    {
        public Guid Id { get; set; }
        public Guid? NurseId { get; set; }
        public int Pulse { get; set; }
        public decimal Temperature { get; set; }
        public string BloodPressure { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public DateTime RecordedAt { get; set; }
    }

    public class Icd10Dto
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }

    public class MedicalExaminationResponseDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string Symptoms { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime ExaminedAt { get; set; }
        public List<Icd10Dto> Icd10Codes { get; set; } = new();
    }

    public class VisitRecordResponseDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid? ReceptionistId { get; set; }
        public Guid? DepartmentId { get; set; }
        public int QueueNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public bool IsPriority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public VitalSignResponseDto? VitalSign { get; set; }
        public MedicalExaminationResponseDto? MedicalExamination { get; set; }
    }
}
