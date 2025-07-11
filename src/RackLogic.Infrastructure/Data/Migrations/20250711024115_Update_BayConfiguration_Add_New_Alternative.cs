using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_BayConfiguration_Add_New_Alternative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAlternativeFrontBeam",
                table: "BayConfigurations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LimitedFrontBeam",
                table: "BayConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlternativeFrontBeam",
                table: "BayConfigurations");

            migrationBuilder.DropColumn(
                name: "LimitedFrontBeam",
                table: "BayConfigurations");
        }
    }
}
