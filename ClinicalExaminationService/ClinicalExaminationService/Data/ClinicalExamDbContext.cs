using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicalExaminationService.Data
{
    public class ClinicalExamDbContext : DbContext
    {
        public ClinicalExamDbContext(DbContextOptions<ClinicalExamDbContext> options) : base(options)
        {
        }

        public DbSet<VisitRecord> VisitRecords { get; set; }
        public DbSet<VitalSign> VitalSigns { get; set; }
        public DbSet<MedicalExamination> MedicalExaminations { get; set; }
        public DbSet<Icd10Catalog> Icd10Catalogs { get; set; }
        public DbSet<ExaminationIcd10> ExaminationIcd10s { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<FollowUpAppointment> FollowUpAppointments { get; set; }
        public DbSet<AssignmentSchedule> AssignmentSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicalExamDbContext).Assembly);
        }
    }
}
