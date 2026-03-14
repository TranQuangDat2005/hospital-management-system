using System.ComponentModel.DataAnnotations;

namespace User_Authentication_Service.DTOs
{
    public class CreatePermissionDto
    {
        [Required(ErrorMessage = "Tên quyền là bắt buộc")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
    }

    public class PermissionResponseDto
    {
        public int PermissionID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class AssignPermissionDto
    {
        [Required(ErrorMessage = "PermissionID là bắt buộc")]
        public int PermissionID { get; set; }
    }

    public class RolePermissionResponseDto
    {
        public int RolePermissionID { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int PermissionID { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionDescription { get; set; } = string.Empty;
    }

    public class RoleSummaryDto
    {
        public string RoleName { get; set; } = string.Empty;
        public List<PermissionResponseDto> Permissions { get; set; } = new();
    }
}
