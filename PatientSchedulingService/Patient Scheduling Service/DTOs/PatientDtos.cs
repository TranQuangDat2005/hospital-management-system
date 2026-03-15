using System.ComponentModel.DataAnnotations;

namespace Patient_Scheduling_Service.DTOs
{
    public class PatientRegistrationDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "CCCD phải bao gồm đúng 12 chữ số.")]
        public string CCCD { get; set; } = string.Empty;

        // Cho phép nhận UserID từ Controller truyền xuống (được trích xuất từ Token)
        public int? UserID { get; set; }
    }

    public class PatientIdentificationDto
    {
        public string? CCCD { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
