using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterInspectionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs");

            migrationBuilder.AddColumn<string>(
                name: "FloorPlanItemInspection",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "DefectLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs");

            migrationBuilder.DropColumn(
                name: "FloorPlanItemInspection",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "DefectLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
