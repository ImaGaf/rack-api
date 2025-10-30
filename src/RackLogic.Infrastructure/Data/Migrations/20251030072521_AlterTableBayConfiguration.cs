using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableBayConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BeamWidth",
                table: "BayConfigurations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableModeLevelSpecificBeam",
                table: "BayConfigurations",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeamWidth",
                table: "BayConfigurations");

            migrationBuilder.DropColumn(
                name: "EnableModeLevelSpecificBeam",
                table: "BayConfigurations");
        }
    }
}
