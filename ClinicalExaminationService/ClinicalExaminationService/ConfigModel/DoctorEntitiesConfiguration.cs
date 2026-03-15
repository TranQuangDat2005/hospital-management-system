using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalExaminationService.ConfigModel
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.ToTable("prescriptions");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.VisitId).IsRequired();
            builder.Property(p => p.DoctorId).IsRequired();
            builder.Property(p => p.PaymentStatus).HasMaxLength(50);
            builder.Property(p => p.DispenseStatus).HasMaxLength(50);

            builder.HasOne(p => p.VisitRecord)
                .WithMany()
                .HasForeignKey(p => p.VisitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PrescriptionItems)
                .WithOne(i => i.Prescription)
                .HasForeignKey(i => i.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
    {
        public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
        {
            builder.ToTable("prescription_items");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.MedicationName).HasMaxLength(150).IsRequired();
            builder.Property(i => i.Dosage).HasMaxLength(100);
        }
    }

    public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
    {
        public void Configure(EntityTypeBuilder<ServiceOrder> builder)
        {
            builder.ToTable("service_orders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.VisitId).IsRequired();
            builder.Property(o => o.DoctorId).IsRequired();
            builder.Property(o => o.ServiceName).HasMaxLength(150);
            builder.Property(o => o.UnitPriceAtOrder).HasColumnType("decimal(15,2)");
            builder.Property(o => o.Status).HasMaxLength(50);

            builder.HasOne(o => o.VisitRecord)
                .WithMany()
                .HasForeignKey(o => o.VisitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FollowUpAppointmentConfiguration : IEntityTypeConfiguration<FollowUpAppointment>
    {
        public void Configure(EntityTypeBuilder<FollowUpAppointment> builder)
        {
            builder.ToTable("follow_up_appointments");
            builder.HasKey(f => f.Id);
            builder.Property(f => f.VisitId).IsRequired();
            builder.Property(f => f.DoctorId).IsRequired();
            builder.Property(f => f.PatientId).IsRequired();
            builder.Property(f => f.FollowUpDate).IsRequired();

            builder.HasOne(f => f.VisitRecord)
                .WithMany()
                .HasForeignKey(f => f.VisitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
