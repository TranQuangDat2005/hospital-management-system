namespace User_Authentication_Service.Model
{
    public class RolePermission
    {
        public int RolePermissionID { get; set; }

        public string RoleName { get; set; } = string.Empty;  

        public int PermissionID { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
