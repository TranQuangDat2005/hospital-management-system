using System;

namespace PharmacyMedicationService.Model
{
    public class PrescriptionItem
    {
        public Guid Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public Guid MedicationId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtOrder { get; set; } // BR-PH-04 Price Integrity
        public ItemStatus Status { get; set; } = ItemStatus.PendingPayment;
        public DateTime UpdatedAt { get; set; }
        
        // Navigation property for query convenience
        public Medication? Medication { get; set; }
    }
}
