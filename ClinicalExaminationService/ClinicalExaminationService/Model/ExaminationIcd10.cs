namespace ClinicalExaminationService.Model
{
    public class ExaminationIcd10
    {
        public Guid Id { get; set; }
        public Guid ExaminationId { get; set; }
        public string Icd10Code { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }

        public MedicalExamination? MedicalExamination { get; set; }
        public Icd10Catalog? Icd10Catalog { get; set; }
    }
}
