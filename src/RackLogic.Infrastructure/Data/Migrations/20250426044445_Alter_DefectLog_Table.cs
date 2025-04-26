using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Alter_DefectLog_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClearanceBetweenFrames",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ClearanceBetweenFrames",
                table: "Components");

            migrationBuilder.AddColumn<string>(
                name: "ComponentType",
                table: "DefectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsXBrace",
                table: "DefectLogs",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentType",
                table: "DefectLogs");

            migrationBuilder.DropColumn(
                name: "IsXBrace",
                table: "DefectLogs");

            migrationBuilder.AddColumn<int>(
                name: "ClearanceBetweenFrames",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClearanceBetweenFrames",
                table: "Components",
                type: "int",
                nullable: true);
        }
    }
}
