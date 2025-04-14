using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Inspection_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_AspNetUsers_AuthorisePersonId",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_FloorPlanItemId",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "AuthorisedPersonId",
                table: "Inspections");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorisePersonId",
                table: "Inspections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InspectionId",
                table: "FloorPlanItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_FloorPlanItemId",
                table: "Inspections",
                column: "FloorPlanItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_AspNetUsers_AuthorisePersonId",
                table: "Inspections",
                column: "AuthorisePersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_AspNetUsers_AuthorisePersonId",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_FloorPlanItemId",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                table: "FloorPlanItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorisePersonId",
                table: "Inspections",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorisedPersonId",
                table: "Inspections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_FloorPlanItemId",
                table: "Inspections",
                column: "FloorPlanItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_AspNetUsers_AuthorisePersonId",
                table: "Inspections",
                column: "AuthorisePersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
