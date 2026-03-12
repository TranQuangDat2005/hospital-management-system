using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.ConfigModel
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Departments>
    {
        void IEntityTypeConfiguration<Departments>.Configure(EntityTypeBuilder<Departments> builder)
        {
            builder.HasKey(d=>d.DepartmentID);
            builder.Property(d => d.Name);
            builder.Property(d => d.Location);
            builder.Property(d => d.Status);
        }
    }
}
