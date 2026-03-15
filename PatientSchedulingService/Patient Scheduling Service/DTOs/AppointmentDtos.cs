using System.ComponentModel.DataAnnotations;

namespace Patient_Scheduling_Service.DTOs
{
    public class AppointmentBookingDto
    {
        [Required]
        public int DoctorID { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public string Symptoms { get; set; } = string.Empty;
    }
}
