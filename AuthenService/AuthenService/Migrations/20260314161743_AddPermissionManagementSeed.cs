using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Authentication_Service.Migrations
{
    public partial class AddPermissionManagementSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionID", "Description", "Name" },
                values: new object[,]
                {
                    { 20, "Xem danh sách quyền và vai trò", "permissions.read" },
                    { 21, "Tạo quyền mới", "permissions.create" },
                    { 22, "Xóa quyền", "permissions.delete" },
                    { 23, "Gán / thu hồi quyền cho vai trò", "permissions.assign" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RolePermissionID", "PermissionID", "RoleName" },
                values: new object[,]
                {
                    { 20, 20, "Admin" },
                    { 21, 21, "Admin" },
                    { 22, 22, "Admin" },
                    { 23, 23, "Admin" }
                });
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RolePermissionID",
                keyValues: new object[] { 20, 21, 22, 23 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValues: new object[] { 20, 21, 22, 23 });
        }
    }
}
