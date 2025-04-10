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
                name: "FK_Inspection_AspNetUsers_AuthorisePersonId",
                table: "Inspection");

            migrationBuilder.DropIndex(
                name: "IX_Inspection_FloorPlanItemId",
                table: "Inspection");

            migrationBuilder.DropColumn(
                name: "AuthorisedPersonId",
                table: "Inspection");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorisePersonId",
                table: "Inspection",
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
                name: "IX_Inspection_FloorPlanItemId",
                table: "Inspection",
                column: "FloorPlanItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspection_AspNetUsers_AuthorisePersonId",
                table: "Inspection",
                column: "AuthorisePersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspection_AspNetUsers_AuthorisePersonId",
                table: "Inspection");

            migrationBuilder.DropIndex(
                name: "IX_Inspection_FloorPlanItemId",
                table: "Inspection");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                table: "FloorPlanItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorisePersonId",
                table: "Inspection",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorisedPersonId",
                table: "Inspection",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_FloorPlanItemId",
                table: "Inspection",
                column: "FloorPlanItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspection_AspNetUsers_AuthorisePersonId",
                table: "Inspection",
                column: "AuthorisePersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
