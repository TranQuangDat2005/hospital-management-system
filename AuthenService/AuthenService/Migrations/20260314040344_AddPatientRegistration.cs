using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Authentication_Service.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RolePermissionID", "PermissionID", "RoleName" },
                values: new object[] { 15, 5, "Patient" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RolePermissionID",
                keyValue: 15);
        }
    }
}
