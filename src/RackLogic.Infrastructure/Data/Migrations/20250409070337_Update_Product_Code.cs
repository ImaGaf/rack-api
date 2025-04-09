using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class Update_Product_Code : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.AlterColumn<string>(
              name: "Code",
              table: "Products",
              type: "nvarchar(max)",
              nullable: true,
              oldClrType: typeof(double),
              oldType: "float",
              oldNullable: true);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.AlterColumn<double>(
              name: "Code",
              table: "Products",
              type: "float",
              nullable: true,
              oldClrType: typeof(string),
              oldType: "nvarchar(max)",
              oldNullable: true);
      }
  }
