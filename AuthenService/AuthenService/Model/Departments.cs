namespace User_Authentication_Service.Model
{
    public class Departments
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }

        public Departments() { }

        public Departments(int departmentID, string name, string location, string status)
        {
            DepartmentID = departmentID;
            Name = name;
            Location = location;
            Status = status;
        }

    }
}
