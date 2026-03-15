namespace ClinicalExaminationService.Model
{
    public class Prescription
    {
        public Guid Id { get; set; }
        public Guid VisitId { get; set; }
        public Guid DoctorId { get; set; }
        public string? Notes { get; set; }
        public string PaymentStatus { get; set; } = "PendingPayment";
        public string DispenseStatus { get; set; } = "PendingDispense";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public VisitRecord? VisitRecord { get; set; }
        public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
    }

    public class PrescriptionItem
    {
        public Guid Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Dosage { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;

        public Prescription? Prescription { get; set; }
    }
}
