using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Components",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UsingFront",
                table: "Components",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "UsingFront",
                table: "Components");
        }
    }
}
