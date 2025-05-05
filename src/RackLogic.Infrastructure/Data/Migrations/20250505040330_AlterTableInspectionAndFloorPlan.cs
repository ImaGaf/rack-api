using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableInspectionAndFloorPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_FloorPlanItems_FloorPlanItemId",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_FloorPlanItemId",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                table: "FloorPlanItems");

            migrationBuilder.RenameColumn(
                name: "FloorPlanItemId",
                table: "Inspections",
                newName: "FloorPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_FloorPlanId",
                table: "Inspections",
                column: "FloorPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_FloorPlans_FloorPlanId",
                table: "Inspections",
                column: "FloorPlanId",
                principalTable: "FloorPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_FloorPlans_FloorPlanId",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_FloorPlanId",
                table: "Inspections");

            migrationBuilder.RenameColumn(
                name: "FloorPlanId",
                table: "Inspections",
                newName: "FloorPlanItemId");

            migrationBuilder.AddColumn<Guid>(
                name: "InspectionId",
                table: "FloorPlanItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_FloorPlanItemId",
                table: "Inspections",
                column: "FloorPlanItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_FloorPlanItems_FloorPlanItemId",
                table: "Inspections",
                column: "FloorPlanItemId",
                principalTable: "FloorPlanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
