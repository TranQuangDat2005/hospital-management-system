namespace ClinicalExaminationService.Model
{
    public class Icd10Catalog
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<ExaminationIcd10> ExaminationIcd10s { get; set; } = new List<ExaminationIcd10>();
    }
}
