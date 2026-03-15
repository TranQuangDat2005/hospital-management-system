using System;

namespace ClinicalExaminationService.Model
{
    public enum ShiftType
    {
        Morning = 0,
        Afternoon = 1,
        Night = 2
    }

    public class AssignmentSchedule
    {
        public Guid Id { get; set; }
        public Guid NurseId { get; set; }
        public Guid DoctorId { get; set; }
        public string ClinicRoom { get; set; } = string.Empty;
        public DateTime ShiftDate { get; set; }
        public ShiftType ShiftType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
