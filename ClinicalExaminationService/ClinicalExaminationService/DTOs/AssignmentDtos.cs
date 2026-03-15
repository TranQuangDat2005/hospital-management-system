using System;

namespace ClinicalExaminationService.DTOs
{
    public class AssignmentScheduleResponseDto
    {
        public Guid Id { get; set; }
        public Guid NurseId { get; set; }
        public Guid DoctorId { get; set; }
        public string ClinicRoom { get; set; } = string.Empty;
        public DateTime ShiftDate { get; set; }
        public string ShiftType { get; set; } = string.Empty;
    }
}
