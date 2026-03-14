using System.ComponentModel.DataAnnotations;

namespace User_Authentication_Service.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Doctor";  // Admin | Doctor | Nurse | Receptionist

        public int DepartmentID { get; set; }

        public string Status { get; set; } = "Active";
    }
}
