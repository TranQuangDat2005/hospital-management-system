using FinancialBillingService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBillingService.ConfigModel
{
    public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
    {
        public void Configure(EntityTypeBuilder<ServiceOrder> builder)
        {
            builder.ToTable("service_orders");
            builder.HasKey(s => s.OrderID);
            
            builder.Property(s => s.OrderID)
                .HasColumnName("order_id")
                .IsRequired();
            builder.Property(s => s.VisitID)
                .HasColumnName("visit_id")
                .IsRequired();
            builder.Property(s => s.ServiceID)
                .HasColumnName("service_id")
                .IsRequired();
            builder.Property(s => s.DoctorID)
                .HasColumnName("doctor_id")
                .IsRequired();
            builder.Property(s => s.UnitPriceAtOrder)
                .HasColumnName("unit_price_at_order")
                .HasColumnType("decimal(15,2)")
                .IsRequired();
            builder.Property(s => s.OrderTime)
                .HasColumnName("order_time")
                .HasColumnType("datetime")
                .IsRequired();
            builder.Property(s => s.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
