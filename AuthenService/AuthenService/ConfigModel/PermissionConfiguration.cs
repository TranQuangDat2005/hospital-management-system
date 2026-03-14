using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.ConfigModel
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.PermissionID);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(255);
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasData(
                new Permission { PermissionID = 1, Name = "users.read",         Description = "Xem danh sách và chi tiết người dùng" },
                new Permission { PermissionID = 2, Name = "users.create",        Description = "Tạo người dùng mới" },
                new Permission { PermissionID = 3, Name = "users.update",        Description = "Cập nhật thông tin người dùng" },
                new Permission { PermissionID = 4, Name = "users.delete",        Description = "Xóa người dùng" },
                new Permission { PermissionID = 5, Name = "departments.read",    Description = "Xem danh sách và chi tiết phòng ban" },
                new Permission { PermissionID = 6, Name = "departments.create",  Description = "Tạo phòng ban mới" },
                new Permission { PermissionID = 7, Name = "departments.update",  Description = "Cập nhật thông tin phòng ban" },
                new Permission { PermissionID = 8, Name = "departments.delete",  Description = "Xóa phòng ban" }
            );
        }
    }
}
