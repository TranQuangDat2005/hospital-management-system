using FinancialBillingService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBillingService.ConfigModel
{
    public class HealthInsuranceConfiguration : IEntityTypeConfiguration<HealthInsurance>
    {
        public void Configure(EntityTypeBuilder<HealthInsurance> builder)
        {
            builder.ToTable("health_insurance");
            builder.HasKey(h => h.InsuranceID);
            
            builder.Property(h => h.InsuranceID)
                .HasColumnName("insurance_id")
                .IsRequired();
            builder.Property(h => h.PatientID)
                .HasColumnName("patient_id")
                .IsRequired();
            builder.Property(h => h.CardNumber)
                .HasColumnName("card_number")
                .HasMaxLength(15)
                .IsRequired();
            builder.Property(h => h.ExpiryDate)
                .HasColumnName("expiry_date")
                .HasColumnType("date")
                .IsRequired();
            builder.Property(h => h.CoverageRate)
                .HasColumnName("coverage_rate")
                .IsRequired();
        }
    }
}
