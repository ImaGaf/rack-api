using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFloorPlanFilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "FloorPlanFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "FloorPlanFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "FloorPlanFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "FloorPlanFiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FloorPlanFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "FloorPlanFiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "FloorPlanFiles",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "FloorPlanFiles");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "FloorPlanFiles");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "FloorPlanFiles");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "FloorPlanFiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FloorPlanFiles");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "FloorPlanFiles");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "FloorPlanFiles");
        }
    }
}
