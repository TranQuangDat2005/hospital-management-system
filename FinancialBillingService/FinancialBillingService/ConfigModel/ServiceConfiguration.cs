using FinancialBillingService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBillingService.ConfigModel
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("services");
            builder.HasKey(s => s.ServiceID);
            
            builder.Property(s => s.ServiceID)
                .HasColumnName("service_id")
                .IsRequired();
            builder.Property(s => s.ServiceName)
                .HasColumnName("service_name")
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(s => s.Category)
                .HasColumnName("category")
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(s => s.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(15,2)")
                .IsRequired();
            builder.Property(s => s.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
