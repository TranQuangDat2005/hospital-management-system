using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patient_Scheduling_Service.Model;

namespace Patient_Scheduling_Service.ConfigModel
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patients>
    {
        public void Configure(EntityTypeBuilder<Patients> builder)
        {
            builder.HasKey(p => p.PatientID);
            builder.Property(p => p.IdentityCard)
                .IsRequired()
                .HasMaxLength(12);
            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.DateOfBirth)
                .IsRequired();
            builder.Property(p => p.Gender)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(p => p.Address)
                .IsRequired();
            builder.Property(p => p.Phone)
                .IsRequired()
                .HasMaxLength(15);
            builder.Property(p => p.EmergencyContact);
            builder.Property(p => p.UserID)
                .IsRequired();
            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
