using System.ComponentModel.DataAnnotations;

namespace Patient_Scheduling_Service.DTOs
{
    public class InsuranceVerificationDto
    {
        [Required]
        public int PatientID { get; set; }

        [Required]
        [RegularExpression(@"^.{15}$", ErrorMessage = "Số thẻ BHYT phải có đúng 15 ký tự (BR7).")]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int CoverageRate { get; set; }

        public string RegisteredHospital { get; set; } = string.Empty;
    }

    public class InsuranceResponseDto
    {
        public int InsuranceID { get; set; }
        public int PatientID { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired => ExpiryDate < DateTime.UtcNow;
        public int CoverageRate { get; set; }
        public string RegisteredHospital { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
    }

    public class AppointmentResponseDto
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int DepartmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string SymptomsDescription { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
