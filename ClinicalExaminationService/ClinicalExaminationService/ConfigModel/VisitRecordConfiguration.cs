using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalExaminationService.ConfigModel
{
    public class VisitRecordConfiguration : IEntityTypeConfiguration<VisitRecord>
    {
        public void Configure(EntityTypeBuilder<VisitRecord> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.PatientId)
                   .IsRequired();

            builder.Property(v => v.QueueNumber)
                   .IsRequired();

            builder.Property(v => v.CheckInTime)
                   .IsRequired();

            builder.Property(v => v.Status)
                   .IsRequired();

            builder.Property(v => v.Reason)
                   .HasMaxLength(500);

            builder.Property(v => v.CreatedAt)
                   .IsRequired();

            builder.Property(v => v.UpdatedAt)
                   .IsRequired();

            builder.HasOne(v => v.VitalSign)
                   .WithOne(vs => vs.VisitRecord)
                   .HasForeignKey<VitalSign>(vs => vs.VisitId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.MedicalExamination)
                   .WithOne(me => me.VisitRecord)
                   .HasForeignKey<MedicalExamination>(me => me.VisitId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
