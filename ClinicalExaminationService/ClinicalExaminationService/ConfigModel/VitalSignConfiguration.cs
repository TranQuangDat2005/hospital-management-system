using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalExaminationService.ConfigModel
{
    public class VitalSignConfiguration : IEntityTypeConfiguration<VitalSign>
    {
        public void Configure(EntityTypeBuilder<VitalSign> builder)
        {
            builder.HasKey(vs => vs.Id);

            builder.Property(vs => vs.VisitId)
                   .IsRequired();

            builder.Property(vs => vs.Pulse)
                   .IsRequired();

            builder.Property(vs => vs.Temperature)
                   .IsRequired()
                   .HasPrecision(4, 2);

            builder.Property(vs => vs.BloodPressure)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(vs => vs.Weight)
                   .IsRequired()
                   .HasPrecision(5, 2);

            builder.Property(vs => vs.RecordedAt)
                   .IsRequired();

            builder.HasIndex(vs => vs.VisitId)
                   .IsUnique();
        }
    }
}
