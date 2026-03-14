using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.ConfigModel
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => rp.RolePermissionID);
            builder.Property(rp => rp.RoleName).IsRequired().HasMaxLength(50);

            builder.HasOne(rp => rp.Permission)
                   .WithMany(p => p.RolePermissions)
                   .HasForeignKey(rp => rp.PermissionID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rp => new { rp.RoleName, rp.PermissionID }).IsUnique();

            // Seed phân quyền mặc định:
            // Admin       → tất cả quyền users + departments
            // Doctor      → read users + departments
            // Nurse       → read users + departments
            // Receptionist→ read users + departments
            builder.HasData(
                // Admin - users
                new RolePermission { RolePermissionID = 1,  RoleName = "Admin",        PermissionID = 1 },
                new RolePermission { RolePermissionID = 2,  RoleName = "Admin",        PermissionID = 2 },
                new RolePermission { RolePermissionID = 3,  RoleName = "Admin",        PermissionID = 3 },
                new RolePermission { RolePermissionID = 4,  RoleName = "Admin",        PermissionID = 4 },
                // Doctor, Nurse, Receptionist - users read
                new RolePermission { RolePermissionID = 5,  RoleName = "Doctor",       PermissionID = 1 },
                new RolePermission { RolePermissionID = 6,  RoleName = "Nurse",        PermissionID = 1 },
                new RolePermission { RolePermissionID = 7,  RoleName = "Receptionist", PermissionID = 1 },
                // Admin - departments (full)
                new RolePermission { RolePermissionID = 8,  RoleName = "Admin",        PermissionID = 5 },
                new RolePermission { RolePermissionID = 9,  RoleName = "Admin",        PermissionID = 6 },
                new RolePermission { RolePermissionID = 10, RoleName = "Admin",        PermissionID = 7 },
                new RolePermission { RolePermissionID = 11, RoleName = "Admin",        PermissionID = 8 },
                // Doctor, Nurse, Receptionist - departments read
                new RolePermission { RolePermissionID = 12, RoleName = "Doctor",       PermissionID = 5 },
                new RolePermission { RolePermissionID = 13, RoleName = "Nurse",        PermissionID = 5 },
                new RolePermission { RolePermissionID = 14, RoleName = "Receptionist", PermissionID = 5 },
                // Patient - chỉ xem danh sách phòng khám (BR11)
                new RolePermission { RolePermissionID = 15, RoleName = "Patient",      PermissionID = 5 }
            );
        }
    }
}
