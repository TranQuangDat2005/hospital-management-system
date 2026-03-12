namespace User_Authentication_Service.Model
{
    public class Users
    {

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public int DepartmentID { get; set; }
        public string Status { get; set; }

        public Users() { }

        public Users(int userID, string userName, string passwordHash, string fullName, string email, string phone, string role, int departmentID, string status)
        {
            UserID = userID;
            UserName = userName;
            PasswordHash = passwordHash;
            FullName = fullName;
            Email = email;
            Phone = phone;
            Role = role;
            DepartmentID = departmentID;
            Status = status;
        }

    }
}
