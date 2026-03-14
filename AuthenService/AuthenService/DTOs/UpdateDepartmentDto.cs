using System.ComponentModel.DataAnnotations;

namespace User_Authentication_Service.DTOs
{
    public class UpdateDepartmentDto
    {
        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // Cho phép Admin thay đổi trạng thái (Inactive ↔ Active)
        [RegularExpression("Active|Inactive", ErrorMessage = "Status chỉ được là 'Active' hoặc 'Inactive'")]
        public string? Status { get; set; }
    }
}
