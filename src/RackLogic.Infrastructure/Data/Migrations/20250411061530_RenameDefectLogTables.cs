using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameDefectLogTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLog_Components_ComponentId",
                table: "DefectLog");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLog_Components_RelatedComponentId",
                table: "DefectLog");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLog_FloorPlanItems_FloorPlanItemId",
                table: "DefectLog");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLog_Inspection_InspectionId",
                table: "DefectLog");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogFile_DefectLog_DefectLogId",
                table: "DefectLogFile");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogHistory_AspNetUsers_UserId",
                table: "DefectLogHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogHistory_DefectLog_DefectLogId",
                table: "DefectLogHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspection_AspNetUsers_AuthorisePersonId",
                table: "Inspection");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspection_FloorPlanItems_FloorPlanItemId",
                table: "Inspection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inspection",
                table: "Inspection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DefectLogHistory",
                table: "DefectLogHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DefectLogFile",
                table: "DefectLogFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DefectLog",
                table: "DefectLog");

            migrationBuilder.RenameTable(
                name: "Inspection",
                newName: "Inspections");

            migrationBuilder.RenameTable(
                name: "DefectLogHistory",
                newName: "DefectLogHistories");

            migrationBuilder.RenameTable(
                name: "DefectLogFile",
                newName: "DefectLogFiles");

            migrationBuilder.RenameTable(
                name: "DefectLog",
                newName: "DefectLogs");

            migrationBuilder.RenameIndex(
                name: "IX_Inspection_FloorPlanItemId",
                table: "Inspections",
                newName: "IX_Inspections_FloorPlanItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Inspection_AuthorisePersonId",
                table: "Inspections",
                newName: "IX_Inspections_AuthorisePersonId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogHistory_UserId",
                table: "DefectLogHistories",
                newName: "IX_DefectLogHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogHistory_DefectLogId",
                table: "DefectLogHistories",
                newName: "IX_DefectLogHistories_DefectLogId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogFile_DefectLogId",
                table: "DefectLogFiles",
                newName: "IX_DefectLogFiles_DefectLogId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLog_RelatedComponentId",
                table: "DefectLogs",
                newName: "IX_DefectLogs_RelatedComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLog_InspectionId",
                table: "DefectLogs",
                newName: "IX_DefectLogs_InspectionId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLog_FloorPlanItemId",
                table: "DefectLogs",
                newName: "IX_DefectLogs_FloorPlanItemId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLog_ComponentId",
                table: "DefectLogs",
                newName: "IX_DefectLogs_ComponentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inspections",
                table: "Inspections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefectLogHistories",
                table: "DefectLogHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefectLogFiles",
                table: "DefectLogFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefectLogs",
                table: "DefectLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogFiles_DefectLogs_DefectLogId",
                table: "DefectLogFiles",
                column: "DefectLogId",
                principalTable: "DefectLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogHistories_AspNetUsers_UserId",
                table: "DefectLogHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogHistories_DefectLogs_DefectLogId",
                table: "DefectLogHistories",
                column: "DefectLogId",
                principalTable: "DefectLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogs_Components_ComponentId",
                table: "DefectLogs",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogs_Components_RelatedComponentId",
                table: "DefectLogs",
                column: "RelatedComponentId",
                principalTable: "Components",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogs_FloorPlanItems_FloorPlanItemId",
                table: "DefectLogs",
                column: "FloorPlanItemId",
                principalTable: "FloorPlanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_AspNetUsers_AuthorisePersonId",
                table: "Inspections",
                column: "AuthorisePersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_FloorPlanItems_FloorPlanItemId",
                table: "Inspections",
                column: "FloorPlanItemId",
                principalTable: "FloorPlanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogFiles_DefectLogs_DefectLogId",
                table: "DefectLogFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogHistories_AspNetUsers_UserId",
                table: "DefectLogHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogHistories_DefectLogs_DefectLogId",
                table: "DefectLogHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogs_Components_ComponentId",
                table: "DefectLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogs_Components_RelatedComponentId",
                table: "DefectLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogs_FloorPlanItems_FloorPlanItemId",
                table: "DefectLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_DefectLogs_Inspections_InspectionId",
                table: "DefectLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_AspNetUsers_AuthorisePersonId",
                table: "Inspections");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_FloorPlanItems_FloorPlanItemId",
                table: "Inspections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inspections",
                table: "Inspections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DefectLogs",
                table: "DefectLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DefectLogHistories",
                table: "DefectLogHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DefectLogFiles",
                table: "DefectLogFiles");

            migrationBuilder.RenameTable(
                name: "Inspections",
                newName: "Inspection");

            migrationBuilder.RenameTable(
                name: "DefectLogs",
                newName: "DefectLog");

            migrationBuilder.RenameTable(
                name: "DefectLogHistories",
                newName: "DefectLogHistory");

            migrationBuilder.RenameTable(
                name: "DefectLogFiles",
                newName: "DefectLogFile");

            migrationBuilder.RenameIndex(
                name: "IX_Inspections_FloorPlanItemId",
                table: "Inspection",
                newName: "IX_Inspection_FloorPlanItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Inspections_AuthorisePersonId",
                table: "Inspection",
                newName: "IX_Inspection_AuthorisePersonId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogs_RelatedComponentId",
                table: "DefectLog",
                newName: "IX_DefectLog_RelatedComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogs_InspectionId",
                table: "DefectLog",
                newName: "IX_DefectLog_InspectionId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogs_FloorPlanItemId",
                table: "DefectLog",
                newName: "IX_DefectLog_FloorPlanItemId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogs_ComponentId",
                table: "DefectLog",
                newName: "IX_DefectLog_ComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogHistories_UserId",
                table: "DefectLogHistory",
                newName: "IX_DefectLogHistory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogHistories_DefectLogId",
                table: "DefectLogHistory",
                newName: "IX_DefectLogHistory_DefectLogId");

            migrationBuilder.RenameIndex(
                name: "IX_DefectLogFiles_DefectLogId",
                table: "DefectLogFile",
                newName: "IX_DefectLogFile_DefectLogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inspection",
                table: "Inspection",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefectLog",
                table: "DefectLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefectLogHistory",
                table: "DefectLogHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefectLogFile",
                table: "DefectLogFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLog_Components_ComponentId",
                table: "DefectLog",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLog_Components_RelatedComponentId",
                table: "DefectLog",
                column: "RelatedComponentId",
                principalTable: "Components",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLog_FloorPlanItems_FloorPlanItemId",
                table: "DefectLog",
                column: "FloorPlanItemId",
                principalTable: "FloorPlanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLog_Inspection_InspectionId",
                table: "DefectLog",
                column: "InspectionId",
                principalTable: "Inspection",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogFile_DefectLog_DefectLogId",
                table: "DefectLogFile",
                column: "DefectLogId",
                principalTable: "DefectLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogHistory_AspNetUsers_UserId",
                table: "DefectLogHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLogHistory_DefectLog_DefectLogId",
                table: "DefectLogHistory",
                column: "DefectLogId",
                principalTable: "DefectLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspection_AspNetUsers_AuthorisePersonId",
                table: "Inspection",
                column: "AuthorisePersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspection_FloorPlanItems_FloorPlanItemId",
                table: "Inspection",
                column: "FloorPlanItemId",
                principalTable: "FloorPlanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
