namespace ClinicalExaminationService.Model
{
    public class FollowUpAppointment
    {
        public Guid Id { get; set; }
        public Guid VisitId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime FollowUpDate { get; set; }
        public string? ClinicalNotes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public VisitRecord? VisitRecord { get; set; }
    }
}
