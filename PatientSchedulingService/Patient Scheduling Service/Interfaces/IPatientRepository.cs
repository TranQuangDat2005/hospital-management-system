using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patients?> GetByIdAsync(int id);
        Task<Patients?> GetByCccdOrPhoneAsync(string? cccd, string? phone);
        Task<bool> IsCccdExistsAsync(string cccd);
        Task<Patients> CreateAsync(Patients patient);
        Task<Patients> UpdateAsync(Patients patient);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> HardDeleteAsync(int id);
    }
}
