namespace User_Authentication_Service.Model
{
    public class Departments
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;   
        public string Status { get; set; } = "Inactive";          
        public bool IsDeleted { get; set; } = false;              
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
