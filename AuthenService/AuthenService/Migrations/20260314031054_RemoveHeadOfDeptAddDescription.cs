using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Authentication_Service.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHeadOfDeptAddDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadOfDept",
                table: "Departments");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Departments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Departments");

            migrationBuilder.AddColumn<string>(
                name: "HeadOfDept",
                table: "Departments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
