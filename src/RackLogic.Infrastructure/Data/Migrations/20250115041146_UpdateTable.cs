using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeleteToken",
                table: "UserOtps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteToken",
                table: "UserOtps");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "AspNetUsers");
        }
    }
}
