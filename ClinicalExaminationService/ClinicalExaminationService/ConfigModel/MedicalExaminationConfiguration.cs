using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalExaminationService.ConfigModel
{
    public class MedicalExaminationConfiguration : IEntityTypeConfiguration<MedicalExamination>
    {
        public void Configure(EntityTypeBuilder<MedicalExamination> builder)
        {
            builder.HasKey(me => me.Id);

            builder.Property(me => me.VisitId)
                   .IsRequired();

            builder.Property(me => me.DoctorId)
                   .IsRequired();

            builder.Property(me => me.Symptoms)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(me => me.Diagnosis)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(me => me.Notes)
                   .HasMaxLength(2000);

            builder.Property(me => me.ExaminedAt)
                   .IsRequired();

            builder.HasMany(me => me.ExaminationIcd10s)
                   .WithOne(e => e.MedicalExamination)
                   .HasForeignKey(e => e.ExaminationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
