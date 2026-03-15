using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patient_Scheduling_Service.Model;
using System.Security.Cryptography.X509Certificates;

namespace Patient_Scheduling_Service.ConfigModel
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointments>
    {
        public void Configure(EntityTypeBuilder<Appointments> builder)
        {
            builder.HasKey(a => a.AppointmentID);
            builder.Property(a => a.PatientID);
            builder.Property(a => a.DoctorID);
            builder.Property(a => a.DepartmentID);
            builder.Property(a => a.AppointmentDate);
            builder.Property(a => a.SymptomsDescription);
            builder.Property(a => a.Status)
                .HasMaxLength(50);
            builder.Property(a => a.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
