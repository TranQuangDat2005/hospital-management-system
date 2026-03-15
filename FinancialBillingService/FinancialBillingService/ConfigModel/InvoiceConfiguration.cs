using FinancialBillingService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBillingService.ConfigModel
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("invoices");
            builder.HasKey(i => i.InvoiceID);
            
            builder.Property(i => i.InvoiceID)
                .HasColumnName("invoice_id")
                .IsRequired();
            builder.Property(i => i.VisitID)
                .HasColumnName("visit_id")
                .IsRequired();
            builder.Property(i => i.CashierID)
                .HasColumnName("cashier_id")
                .IsRequired();
            builder.Property(i => i.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("decimal(15,2)")
                .IsRequired();
            builder.Property(i => i.InsurancePaid)
                .HasColumnName("insurance_paid")
                .HasColumnType("decimal(15,2)")
                .IsRequired();
            builder.Property(i => i.PatientPaid)
                .HasColumnName("patient_paid")
                .HasColumnType("decimal(15,2)")
                .IsRequired();
            builder.Property(i => i.PaymentMethod)
                .HasColumnName("payment_method")
                .HasMaxLength(50);
            builder.Property(i => i.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(i => i.IssuedAt)
                .HasColumnName("issued_at")
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
