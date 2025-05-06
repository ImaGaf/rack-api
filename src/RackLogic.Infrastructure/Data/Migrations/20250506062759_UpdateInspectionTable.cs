using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInspectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authorise",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "BayId",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Inspections");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "Inspections",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Inspections");

            migrationBuilder.AddColumn<string>(
                name: "Authorise",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BayId",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
