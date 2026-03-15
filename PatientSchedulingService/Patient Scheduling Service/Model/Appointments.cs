namespace Patient_Scheduling_Service.Model
{
    public class Appointments
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int DepartmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string SymptomsDescription{ get; set; }
        public string Status { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Appointments() { }

        public Appointments(int appointmentID, int patientID, int doctorID, int departmentID, DateTime appointmentDate, string symptomsDescription, string status)
        {
            AppointmentID = appointmentID;
            PatientID = patientID;
            DoctorID = doctorID;
            DepartmentID = departmentID;
            AppointmentDate = appointmentDate;
            SymptomsDescription = symptomsDescription;
            Status = status;
        }
    }
}
