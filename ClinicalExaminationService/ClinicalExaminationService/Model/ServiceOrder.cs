namespace ClinicalExaminationService.Model
{
    public class ServiceOrder
    {
        public Guid Id { get; set; }
        public Guid VisitId { get; set; }
        public Guid DoctorId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal UnitPriceAtOrder { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Ordered";

        public VisitRecord? VisitRecord { get; set; }
    }
}
