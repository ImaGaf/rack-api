using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableWarehouseColumnAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SiteAddress",
                table: "Warehouses",
                newName: "Address");

            migrationBuilder.AddColumn<string>(
                name: "AddressOptional",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGoogleApiAddress",
                table: "Warehouses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressOptional",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "IsGoogleApiAddress",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Warehouses");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Warehouses",
                newName: "SiteAddress");
        }
    }
}
