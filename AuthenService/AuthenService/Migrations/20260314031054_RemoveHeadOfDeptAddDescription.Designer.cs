using System;
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
    [Migration("20260314031054_RemoveHeadOfDeptAddDescription")]
    partial class RemoveHeadOfDeptAddDescription
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

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasDefaultValue("Inactive");

                    b.HasKey("DepartmentID");

                    b.HasIndex("Name")
                        .IsUnique();

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
                        },
                        new
                        {
                            PermissionID = 5,
                            Description = "Xem danh sách và chi tiết phòng ban",
                            Name = "departments.read"
                        },
                        new
                        {
                            PermissionID = 6,
                            Description = "Tạo phòng ban mới",
                            Name = "departments.create"
                        },
                        new
                        {
                            PermissionID = 7,
                            Description = "Cập nhật thông tin phòng ban",
                            Name = "departments.update"
                        },
                        new
                        {
                            PermissionID = 8,
                            Description = "Xóa phòng ban",
                            Name = "departments.delete"
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
                        },
                        new
                        {
                            RolePermissionID = 8,
                            PermissionID = 5,
                            RoleName = "Admin"
                        },
                        new
                        {
                            RolePermissionID = 9,
                            PermissionID = 6,
                            RoleName = "Admin"
                        },
                        new
                        {
                            RolePermissionID = 10,
                            PermissionID = 7,
                            RoleName = "Admin"
                        },
                        new
                        {
                            RolePermissionID = 11,
                            PermissionID = 8,
                            RoleName = "Admin"
                        },
                        new
                        {
                            RolePermissionID = 12,
                            PermissionID = 5,
                            RoleName = "Doctor"
                        },
                        new
                        {
                            RolePermissionID = 13,
                            PermissionID = 5,
                            RoleName = "Nurse"
                        },
                        new
                        {
                            RolePermissionID = 14,
                            PermissionID = 5,
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

                    b.HasIndex("DepartmentID");

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

            modelBuilder.Entity("User_Authentication_Service.Model.Users", b =>
                {
                    b.HasOne("User_Authentication_Service.Model.Departments", null)
                        .WithMany("Users")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("User_Authentication_Service.Model.Departments", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("User_Authentication_Service.Model.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
