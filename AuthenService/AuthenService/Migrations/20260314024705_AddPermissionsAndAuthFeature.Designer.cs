using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using User_Authentication_Service.Data;

#nullable disable

namespace User_Authentication_Service.Migrations
{
    [DbContext(typeof(UserAuthenDbContext))]
    [Migration("20260314024705_AddPermissionsAndAuthFeature")]
    partial class AddPermissionsAndAuthFeature
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("User_Authentication_Service.Model.Departments", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DepartmentID"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("User_Authentication_Service.Model.Permission", b =>
                {
                    b.Property<int>("PermissionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PermissionID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("PermissionID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            PermissionID = 1,
                            Description = "Xem danh sách và chi tiết người dùng",
                            Name = "users.read"
                        },
                        new
                        {
                            PermissionID = 2,
                            Description = "Tạo người dùng mới",
                            Name = "users.create"
                        },
                        new
                        {
                            PermissionID = 3,
                            Description = "Cập nhật thông tin người dùng",
                            Name = "users.update"
                        },
                        new
                        {
                            PermissionID = 4,
                            Description = "Xóa người dùng",
                            Name = "users.delete"
                        });
                });

            modelBuilder.Entity("User_Authentication_Service.Model.RolePermission", b =>
                {
                    b.Property<int>("RolePermissionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RolePermissionID"));

                    b.Property<int>("PermissionID")
                        .HasColumnType("integer");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("RolePermissionID");

                    b.HasIndex("PermissionID");

                    b.HasIndex("RoleName", "PermissionID")
                        .IsUnique();

                    b.ToTable("RolePermissions");

                    b.HasData(
                        new
                        {
                            RolePermissionID = 1,
                            PermissionID = 1,
                            RoleName = "Admin"
                        },
                        new
                        {
                            RolePermissionID = 2,
                            PermissionID = 2,
                            RoleName = "Admin"
                        },
                        new
                        {
                            RolePermissionID = 3,
                            PermissionID = 3,
                            RoleName = "Admin"
                        },
                        new
                        {
                            RolePermissionID = 4,
                            PermissionID = 4,
                            RoleName = "Admin"
                        },
                        new
                        {
                            RolePermissionID = 5,
                            PermissionID = 1,
                            RoleName = "Doctor"
                        },
                        new
                        {
                            RolePermissionID = 6,
                            PermissionID = 1,
                            RoleName = "Nurse"
                        },
                        new
                        {
                            RolePermissionID = 7,
                            PermissionID = 1,
                            RoleName = "Receptionist"
                        });
                });

            modelBuilder.Entity("User_Authentication_Service.Model.Users", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserID"));

                    b.Property<int>("DepartmentID")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("User_Authentication_Service.Model.RolePermission", b =>
                {
                    b.HasOne("User_Authentication_Service.Model.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("User_Authentication_Service.Model.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
