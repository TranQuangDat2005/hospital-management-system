using System.ComponentModel.DataAnnotations;

namespace User_Authentication_Service.Model
{
    public class Permission
    {
        public int PermissionID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
