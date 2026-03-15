namespace ClinicalExaminationService.Model
{
    public class MedicalExamination
    {
        public Guid Id { get; set; }
        public Guid VisitId { get; set; }
        public Guid DoctorId { get; set; }
        public string Symptoms { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime ExaminedAt { get; set; } = DateTime.UtcNow;

        public VisitRecord? VisitRecord { get; set; }
        public ICollection<ExaminationIcd10> ExaminationIcd10s { get; set; } = new List<ExaminationIcd10>();
    }
}
