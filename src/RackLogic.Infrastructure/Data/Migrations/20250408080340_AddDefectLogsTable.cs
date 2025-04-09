using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefectLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Companyname",
                table: "InspectorProfiles",
                newName: "CompanyName");

            migrationBuilder.CreateTable(
                name: "DefectLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FloorPlanItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefectGrade = table.Column<int>(type: "int", nullable: true),
                    DefectPriority = table.Column<int>(type: "int", nullable: true),
                    DefectType = table.Column<int>(type: "int", nullable: true),
                    CorrectiveAction = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefectLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefectLog_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefectLog_Components_RelatedComponentId",
                        column: x => x.RelatedComponentId,
                        principalTable: "Components",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DefectLog_FloorPlanItems_FloorPlanItemId",
                        column: x => x.FloorPlanItemId,
                        principalTable: "FloorPlanItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefectLogFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefectLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefectLogFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefectLogFiles_DefectLog_DefectLogId",
                        column: x => x.DefectLogId,
                        principalTable: "DefectLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefectLog_ComponentId",
                table: "DefectLog",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_DefectLog_FloorPlanItemId",
                table: "DefectLog",
                column: "FloorPlanItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DefectLog_RelatedComponentId",
                table: "DefectLog",
                column: "RelatedComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_DefectLogFiles_DefectLogId",
                table: "DefectLogFiles",
                column: "DefectLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefectLogFiles");

            migrationBuilder.DropTable(
                name: "DefectLog");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "InspectorProfiles",
                newName: "Companyname");
        }
    }
}
