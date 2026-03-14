namespace User_Authentication_Service.DTOs
{
    public class UserResponseDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int DepartmentID { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
