using Microsoft.EntityFrameworkCore;
using PharmacyMedicationService.Model;

namespace PharmacyMedicationService.Data
{
    public class PharmacyMedicationDbContext : DbContext
    {
        public PharmacyMedicationDbContext(DbContextOptions<PharmacyMedicationDbContext> options) : base(options) { }

        public DbSet<Medication> Medications { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrescriptionItem>()
                .HasOne(p => p.Medication)
                .WithMany()
                .HasForeignKey(p => p.MedicationId);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}
