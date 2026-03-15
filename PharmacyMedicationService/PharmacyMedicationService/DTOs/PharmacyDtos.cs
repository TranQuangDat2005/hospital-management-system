using System;
using System.Collections.Generic;
using PharmacyMedicationService.Model;

namespace PharmacyMedicationService.DTOs
{
    public class UpdateItemStatusDto
    {
        public List<Guid> ItemIds { get; set; } = new List<Guid>();
        public ItemStatus NewStatus { get; set; }
    }

    public class PrescriptionItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public Guid MedicationId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPriceAtOrder { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}
