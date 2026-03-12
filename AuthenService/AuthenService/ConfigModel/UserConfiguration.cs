using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.ConfigModel
{
    public class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(u=> u.UserID);
            builder.Property(u => u.UserName);
            builder.Property(u => u.PasswordHash);
            builder.Property(u => u.FullName);
            builder.Property(u => u.Email);
            builder.Property(u => u.Phone);
            builder.Property(u => u.Role);
            builder.Property(u => u.DepartmentID);
            builder.Property(u => u.Status);
        }
    }
}