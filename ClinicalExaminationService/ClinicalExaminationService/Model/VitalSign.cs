namespace ClinicalExaminationService.Model
{
    public class VitalSign
    {
        public Guid Id { get; set; }
        public Guid VisitId { get; set; }
        public Guid? NurseId { get; set; }
        public int Pulse { get; set; }
        public decimal Temperature { get; set; }
        public string BloodPressure { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        public VisitRecord? VisitRecord { get; set; }
    }
}
