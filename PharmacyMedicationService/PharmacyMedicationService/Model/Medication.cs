using System;

namespace PharmacyMedicationService.Model
{
    public class Medication
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public bool Status { get; set; } // Active/Inactive
    }
}
