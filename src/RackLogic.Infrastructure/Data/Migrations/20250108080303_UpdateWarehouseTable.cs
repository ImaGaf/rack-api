using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWarehouseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Warehouses",
                newName: "SiteAddress");

            migrationBuilder.AddColumn<string>(
                name: "ABN",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientPhoneNumber",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileImageName",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileImageUrl",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerEmailAddress",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerFirstName",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerLastName",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerPhoneNumber",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ABN",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "ClientPhoneNumber",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "FileImageName",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "FileImageUrl",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "OwnerEmailAddress",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "OwnerFirstName",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "OwnerLastName",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "OwnerPhoneNumber",
                table: "Warehouses");

            migrationBuilder.RenameColumn(
                name: "SiteAddress",
                table: "Warehouses",
                newName: "Name");
        }
    }
}
