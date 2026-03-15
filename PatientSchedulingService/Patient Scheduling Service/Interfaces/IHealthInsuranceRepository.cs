using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Interfaces
{
    public interface IHealthInsuranceRepository
    {
        Task<HealthInsurance?> GetByPatientIdAsync(int patientId);
        Task<HealthInsurance> UpsertAsync(HealthInsurance insurance);
    }
}
