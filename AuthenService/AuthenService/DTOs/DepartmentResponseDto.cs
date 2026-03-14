namespace User_Authentication_Service.DTOs
{
    public class DepartmentResponseDto
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ActiveUserCount { get; set; }  // Số nhân viên đang active - hỗ trợ dependency check UI
    }
}
