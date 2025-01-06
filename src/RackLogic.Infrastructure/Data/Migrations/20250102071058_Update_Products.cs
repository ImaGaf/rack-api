using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class Update_Products : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.AddColumn<double>(
              name: "Length",
              table: "Products",
              type: "float",
              nullable: true);

          migrationBuilder.AddColumn<string>(
              name: "Orientation",
              table: "Products",
              type: "nvarchar(max)",
              nullable: true);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropColumn(
              name: "Length",
              table: "Products");

          migrationBuilder.DropColumn(
              name: "Orientation",
              table: "Products");
      }
  }
