using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.ConfigModel
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Departments>
    {
        public void Configure(EntityTypeBuilder<Departments> builder)
        {
            builder.HasKey(d => d.DepartmentID);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(d => d.Location)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(d => d.Description)
                   .HasMaxLength(500);

            builder.Property(d => d.Status)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasDefaultValue("Inactive");

            builder.Property(d => d.IsDeleted)
                   .HasDefaultValue(false);

            builder.Property(d => d.CreatedAt)
                   .HasDefaultValueSql("NOW()");

            // Unique index: tên phòng ban không được trùng (xác thực theo spec)
            builder.HasIndex(d => d.Name).IsUnique();

            // Global query filter: tự động ẩn record đã soft-delete
            builder.HasQueryFilter(d => !d.IsDeleted);

            // FK: Users → Departments (ON DELETE RESTRICT)
            // Đây chính là dependency check - DB chặn xóa nếu còn user gắn với dept này
            builder.HasMany(d => d.Users)
                   .WithOne()
                   .HasForeignKey(u => u.DepartmentID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
