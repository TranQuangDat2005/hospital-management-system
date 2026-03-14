namespace User_Authentication_Service.DTOs
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public int? DepartmentID { get; set; }
        public string? Status { get; set; }
        public string? Password { get; set; }  // null = không đổi password
    }
}
