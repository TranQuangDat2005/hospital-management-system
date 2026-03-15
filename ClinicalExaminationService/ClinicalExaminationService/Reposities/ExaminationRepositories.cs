using ClinicalExaminationService.Data;
using ClinicalExaminationService.Interfaces;
using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicalExaminationService.Reposities
{
    public class VisitRecordRepository : IVisitRecordRepository
    {
        private readonly ClinicalExamDbContext _context;

        public VisitRecordRepository(ClinicalExamDbContext context)
        {
            _context = context;
        }

        public async Task<VisitRecord> AddAsync(VisitRecord visitRecord)
        {
            _context.VisitRecords.Add(visitRecord);
            await _context.SaveChangesAsync();
            return visitRecord;
        }

        public async Task<IEnumerable<VisitRecord>> GetAllAsync(VisitStatus? status = null)
        {
            var query = _context.VisitRecords
                .Include(v => v.VitalSign)
                .Include(v => v.MedicalExamination)
                .ThenInclude(m => m!.ExaminationIcd10s)
                .ThenInclude(e => e.Icd10Catalog)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(v => v.Status == status.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<VisitRecord?> GetByIdAsync(Guid id)
        {
            return await _context.VisitRecords
                .Include(v => v.VitalSign)
                .Include(v => v.MedicalExamination)
                .ThenInclude(m => m!.ExaminationIcd10s)
                .ThenInclude(e => e.Icd10Catalog)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<VisitRecord> UpdateAsync(VisitRecord visitRecord)
        {
            visitRecord.UpdatedAt = DateTime.UtcNow;
            _context.VisitRecords.Update(visitRecord);
            await _context.SaveChangesAsync();
            return visitRecord;
        }

        public async Task<int> GetMaxQueueNumberAsync(Guid? departmentId, DateTime date)
        {
            var maxQueueNumber = await _context.VisitRecords
                .Where(v => v.DepartmentId == departmentId && v.CheckInTime.Date == date.Date)
                .MaxAsync(v => (int?)v.QueueNumber) ?? 0;

            return maxQueueNumber;
        }
    }

    public class VitalSignRepository : IVitalSignRepository
    {
        private readonly ClinicalExamDbContext _context;

        public VitalSignRepository(ClinicalExamDbContext context)
        {
            _context = context;
        }

        public async Task<VitalSign> AddAsync(VitalSign vitalSign)
        {
            _context.VitalSigns.Add(vitalSign);
            await _context.SaveChangesAsync();
            return vitalSign;
        }

        public async Task<VitalSign?> GetByVisitIdAsync(Guid visitId)
        {
            return await _context.VitalSigns.FirstOrDefaultAsync(v => v.VisitId == visitId);
        }
    }

    public class MedicalExaminationRepository : IMedicalExaminationRepository
    {
        private readonly ClinicalExamDbContext _context;

        public MedicalExaminationRepository(ClinicalExamDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalExamination> AddAsync(MedicalExamination examination)
        {
            _context.MedicalExaminations.Add(examination);
            await _context.SaveChangesAsync();
            return examination;
        }

        public async Task<MedicalExamination?> GetByVisitIdAsync(Guid visitId)
        {
            return await _context.MedicalExaminations
                .Include(m => m.ExaminationIcd10s)
                .ThenInclude(e => e.Icd10Catalog)
                .FirstOrDefaultAsync(m => m.VisitId == visitId);
        }

        public async Task<MedicalExamination> UpdateAsync(MedicalExamination examination)
        {
            _context.MedicalExaminations.Update(examination);
            await _context.SaveChangesAsync();
            return examination;
        }
    }

    public class Icd10CatalogRepository : IIcd10CatalogRepository
    {
        private readonly ClinicalExamDbContext _context;

        public Icd10CatalogRepository(ClinicalExamDbContext context)
        {
            _context = context;
        }

        public async Task<Icd10Catalog?> GetByCodeAsync(string code)
        {
            return await _context.Icd10Catalogs.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<IEnumerable<Icd10Catalog>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                
                return await _context.Icd10Catalogs.Take(50).ToListAsync();
            }

            keyword = keyword.ToLower();
            return await _context.Icd10Catalogs
                .Where(c => c.Code.ToLower().Contains(keyword) || c.Description.ToLower().Contains(keyword))
                .Take(50)
                .ToListAsync();
        }
    }

    public class AssignmentScheduleRepository : IAssignmentScheduleRepository
    {
        private readonly ClinicalExamDbContext _context;

        public AssignmentScheduleRepository(ClinicalExamDbContext context)
        {
            _context = context;
        }

        public async Task<AssignmentSchedule> AddAsync(AssignmentSchedule assignment)
        {
            _context.AssignmentSchedules.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<IEnumerable<AssignmentSchedule>> GetByNurseIdAsync(Guid nurseId, DateTime? date = null)
        {
            var query = _context.AssignmentSchedules.Where(a => a.NurseId == nurseId);

            if (date.HasValue)
            {
                query = query.Where(a => a.ShiftDate.Date == date.Value.Date);
            }

            return await query.OrderBy(a => a.ShiftDate).ThenBy(a => a.ShiftType).ToListAsync();
        }
    }
}

