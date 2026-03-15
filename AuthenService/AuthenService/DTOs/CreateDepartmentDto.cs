using System.ComponentModel.DataAnnotations;

namespace User_Authentication_Service.DTOs
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Tên phòng ban là bắt buộc")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vị trí là bắt buộc")]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
