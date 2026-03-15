using ClinicalExaminationService.Model;

namespace ClinicalExaminationService.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<Prescription?> GetByVisitIdAsync(Guid visitId);
        Task<Prescription?> GetByIdAsync(Guid id);
        Task<Prescription> AddAsync(Prescription prescription);
        Task<Prescription> UpdateAsync(Prescription prescription);
    }

    public interface IServiceOrderRepository
    {
        Task<IEnumerable<ServiceOrder>> GetByVisitIdAsync(Guid visitId);
        Task<ServiceOrder> AddAsync(ServiceOrder order);
    }

    public interface IFollowUpAppointmentRepository
    {
        Task<FollowUpAppointment> AddAsync(FollowUpAppointment followUp);
        Task<IEnumerable<FollowUpAppointment>> GetByPatientIdAsync(Guid patientId);
    }
}
