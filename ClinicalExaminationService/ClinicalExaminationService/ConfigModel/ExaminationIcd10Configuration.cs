using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalExaminationService.ConfigModel
{
    public class ExaminationIcd10Configuration : IEntityTypeConfiguration<ExaminationIcd10>
    {
        public void Configure(EntityTypeBuilder<ExaminationIcd10> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ExaminationId)
                   .IsRequired();

            builder.Property(e => e.Icd10Code)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(e => e.IsPrimary)
                   .IsRequired();

            builder.HasOne(e => e.Icd10Catalog)
                   .WithMany(c => c.ExaminationIcd10s)
                   .HasForeignKey(e => e.Icd10Code)
                   .HasPrincipalKey(c => c.Code)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.ExaminationId, e.Icd10Code })
                   .IsUnique();
        }
    }
}
