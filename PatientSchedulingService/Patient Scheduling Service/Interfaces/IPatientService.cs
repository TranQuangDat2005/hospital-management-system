using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Interfaces
{
    public interface IPatientService
    {
        Task<Patients> RegisterPatientAsync(PatientRegistrationDto dto);
        Task<Patients?> IdentifyPatientAsync(string? cccd, string? phone);
        Task<Patients> UpdatePatientProfileAsync(int patientId, PatientRegistrationDto dto);
    }
}
