using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.ConfigModel
{
    public class HealthInsuranceConfiguration : IEntityTypeConfiguration<HealthInsurance>
    {
        public void Configure(EntityTypeBuilder<HealthInsurance> builder)
        {
            builder.ToTable("health_insurance");
            builder.HasKey(h => h.InsuranceID);
            builder.Property(h => h.InsuranceID).HasColumnName("insurance_id").ValueGeneratedOnAdd();
            builder.Property(h => h.PatientID).HasColumnName("patient_id").IsRequired();
            builder.Property(h => h.CardNumber).HasColumnName("card_number").HasMaxLength(15).IsRequired();
            builder.Property(h => h.ExpiryDate).HasColumnName("expiry_date").IsRequired();
            builder.Property(h => h.CoverageRate).HasColumnName("coverage_rate").IsRequired();
            builder.Property(h => h.RegisteredHospital).HasColumnName("registered_hospital").HasMaxLength(200);
            builder.Property(h => h.IsVerified).HasColumnName("is_verified").HasDefaultValue(false);

            builder.HasOne(h => h.Patient)
                .WithMany()
                .HasForeignKey(h => h.PatientID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
