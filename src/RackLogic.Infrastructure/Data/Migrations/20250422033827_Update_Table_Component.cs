using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Table_Component : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepthOfFrame",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "BeamStyle",
                table: "Components",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClearEntryWidth",
                table: "Components",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClearanceBetweenFrames",
                table: "Components",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectorType",
                table: "Components",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinishType",
                table: "Components",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialThickness",
                table: "Components",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "XBraceLevel",
                table: "BayConfigurations",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeamStyle",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ClearEntryWidth",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ClearanceBetweenFrames",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ConnectorType",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "FinishType",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "MaterialThickness",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "XBraceLevel",
                table: "BayConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "DepthOfFrame",
                table: "Products",
                type: "int",
                nullable: true);
        }
    }
}
