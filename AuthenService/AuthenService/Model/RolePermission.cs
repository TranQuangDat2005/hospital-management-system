namespace User_Authentication_Service.Model
{
    public class RolePermission
    {
        public int RolePermissionID { get; set; }

        public string RoleName { get; set; } = string.Empty;  // vd: "Admin", "Doctor", "Nurse"

        public int PermissionID { get; set; }

        // Navigation
        public Permission Permission { get; set; } = null!;
    }
}
