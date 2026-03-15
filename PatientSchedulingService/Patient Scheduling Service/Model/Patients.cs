using System.Runtime.CompilerServices;

namespace Patient_Scheduling_Service.Model
{
    public class Patients
    {

        public int PatientID { get; set; }
        public string IdentityCard { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string EmergencyContact { get; set; }
        public int UserID { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Patients(int patientID, string identityCard, string fullName, DateTime dateOfBirth, string gender, string address, string phone, string emergencyContact, int userID)
        {
            PatientID = patientID;
            IdentityCard = identityCard;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            EmergencyContact = emergencyContact;
            UserID = userID;
        }

        public Patients()
        {
        }
    }
}
