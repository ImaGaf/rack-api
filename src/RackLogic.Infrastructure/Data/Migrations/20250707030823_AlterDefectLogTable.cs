using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
  /// <inheritdoc />
  public partial class AlterDefectLogTable : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_DefectLogs_Components_RelatedComponentId",
          table: "DefectLogs");

      migrationBuilder.DropIndex(
          name: "IX_DefectLogs_RelatedComponentId",
          table: "DefectLogs");

      migrationBuilder.DropColumn(
          name: "RelatedComponentId",
          table: "DefectLogs");

      migrationBuilder.AddColumn<string>(
          name: "RelatedComponent",
          table: "DefectLogs",
          type: "nvarchar(max)",
          nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "RelatedComponent",
          table: "DefectLogs");

      migrationBuilder.AddColumn<Guid>(
          name: "RelatedComponentId",
          table: "DefectLogs",
          type: "uniqueidentifier",
          nullable: true);

      migrationBuilder.CreateIndex(
          name: "IX_DefectLogs_RelatedComponentId",
          table: "DefectLogs",
          column: "RelatedComponentId");

      migrationBuilder.AddForeignKey(
          name: "FK_DefectLogs_Components_RelatedComponentId",
          table: "DefectLogs",
          column: "RelatedComponentId",
          principalTable: "Components",
          principalColumn: "Id");
    }
  }
}
