using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class RenamePropertyTableFloorPlan : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.RenameColumn(
              name: "totalRack",
              table: "FloorPlans",
              newName: "TotalRack");

          migrationBuilder.RenameColumn(
              name: "totalBay",
              table: "FloorPlans",
              newName: "TotalBay");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.RenameColumn(
              name: "TotalRack",
              table: "FloorPlans",
              newName: "totalRack");

          migrationBuilder.RenameColumn(
              name: "TotalBay",
              table: "FloorPlans",
              newName: "totalBay");
      }
  }
