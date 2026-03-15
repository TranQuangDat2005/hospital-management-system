using ClinicalExaminationService.Model;

namespace ClinicalExaminationService.Interfaces
{
    public interface IVisitRecordRepository
    {
        Task<VisitRecord?> GetByIdAsync(Guid id);
        Task<IEnumerable<VisitRecord>> GetAllAsync(VisitStatus? status = null);
        Task<VisitRecord> AddAsync(VisitRecord visitRecord);
        Task<VisitRecord> UpdateAsync(VisitRecord visitRecord);
        Task<int> GetMaxQueueNumberAsync(Guid? departmentId, DateTime date);
    }

    public interface IVitalSignRepository
    {
        Task<VitalSign?> GetByVisitIdAsync(Guid visitId);
        Task<VitalSign> AddAsync(VitalSign vitalSign);
    }

    public interface IMedicalExaminationRepository
    {
        Task<MedicalExamination?> GetByVisitIdAsync(Guid visitId);
        Task<MedicalExamination> AddAsync(MedicalExamination examination);
        Task<MedicalExamination> UpdateAsync(MedicalExamination examination);
    }

    public interface IIcd10CatalogRepository
    {
        Task<IEnumerable<Icd10Catalog>> SearchAsync(string keyword);
        Task<Icd10Catalog?> GetByCodeAsync(string code);
    }

    public interface IAssignmentScheduleRepository
    {
        Task<IEnumerable<AssignmentSchedule>> GetByNurseIdAsync(Guid nurseId, DateTime? date = null);
        Task<AssignmentSchedule> AddAsync(AssignmentSchedule assignment);
    }
}
