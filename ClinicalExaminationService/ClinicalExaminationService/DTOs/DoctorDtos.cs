using System.ComponentModel.DataAnnotations;

namespace ClinicalExaminationService.DTOs
{
    public class CreatePrescriptionDto
    {
        public string? Notes { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "A prescription must have at least one item.")]
        public List<CreatePrescriptionItemDto> Items { get; set; } = new();
    }

    public class CreatePrescriptionItemDto
    {
        [Required]
        public string MedicationName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }

        public string Dosage { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
    }

    public class CreateServiceOrderDto
    {
        [Required]
        public int ServiceId { get; set; }

        [Required]
        public string ServiceName { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPriceAtOrder { get; set; }
    }

    public class CreateFollowUpDto
    {
        [Required]
        public DateTime FollowUpDate { get; set; }

        public string? ClinicalNotes { get; set; }
    }
}
