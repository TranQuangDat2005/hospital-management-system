using Microsoft.EntityFrameworkCore;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.Data
{
    public class PatientScheduleDbContext : DbContext
    {
        public PatientScheduleDbContext(DbContextOptions<PatientScheduleDbContext> options) : base(options)
        {
        }

        public DbSet<Patients> Patients { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<HealthInsurance> HealthInsurances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PatientScheduleDbContext).Assembly);
        }
    }
}
