namespace ClinicalExaminationService.Model
{
    public enum VisitStatus
    {
        Waiting = 0,
        VitalCheck = 1,
        Examining = 2,
        PendingPayment = 3,
        PaidPendingMeds = 4,
        Closed = 5
    }

    public class VisitRecord
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid? ReceptionistId { get; set; }
        public Guid? DepartmentId { get; set; }
        public int QueueNumber { get; set; }
        public DateTime CheckInTime { get; set; } = DateTime.UtcNow;
        public VisitStatus Status { get; set; } = VisitStatus.Waiting;
        public string? Reason { get; set; }
        public bool IsPriority { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public VitalSign? VitalSign { get; set; }
        public MedicalExamination? MedicalExamination { get; set; }
    }
}
