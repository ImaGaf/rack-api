using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class Update_Component_Add_Column_Kit : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropColumn(
              name: "SpliceKits",
              table: "BayConfigurations");

          migrationBuilder.AddColumn<string>(
              name: "ComponentKit",
              table: "Components",
              type: "nvarchar(max)",
              nullable: true);

          migrationBuilder.AddColumn<int>(
              name: "Kit",
              table: "Components",
              type: "int",
              nullable: true);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropColumn(
              name: "ComponentKit",
              table: "Components");

          migrationBuilder.DropColumn(
              name: "Kit",
              table: "Components");

          migrationBuilder.AddColumn<string>(
              name: "SpliceKits",
              table: "BayConfigurations",
              type: "nvarchar(max)",
              nullable: true);
      }
  }
