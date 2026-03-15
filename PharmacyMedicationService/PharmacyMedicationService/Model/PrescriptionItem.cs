using System;

namespace PharmacyMedicationService.Model
{
    public class PrescriptionItem
    {
        public Guid Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public Guid MedicationId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtOrder { get; set; } 
        public ItemStatus Status { get; set; } = ItemStatus.PendingPayment;
        public DateTime UpdatedAt { get; set; }
        
        
        public Medication? Medication { get; set; }
    }
}
