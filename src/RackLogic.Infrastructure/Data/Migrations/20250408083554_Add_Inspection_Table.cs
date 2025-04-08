using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Inspection_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InspectionId",
                table: "DefectLog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Inspection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FloorPlanItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorisedPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextInspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BayId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Authorise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConfirmAuthorization = table.Column<bool>(type: "bit", nullable: false),
                    AuthorisePersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspection_AspNetUsers_AuthorisePersonId",
                        column: x => x.AuthorisePersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inspection_FloorPlanItems_FloorPlanItemId",
                        column: x => x.FloorPlanItemId,
                        principalTable: "FloorPlanItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefectLog_InspectionId",
                table: "DefectLog",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_AuthorisePersonId",
                table: "Inspection",
                column: "AuthorisePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_FloorPlanItemId",
                table: "Inspection",
                column: "FloorPlanItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLog_Inspection_InspectionId",
                table: "DefectLog",
                column: "InspectionId",
                principalTable: "Inspection",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLog_Inspection_InspectionId",
                table: "DefectLog");

            migrationBuilder.DropTable(
                name: "Inspection");

            migrationBuilder.DropIndex(
                name: "IX_DefectLog_InspectionId",
                table: "DefectLog");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                table: "DefectLog");
        }
    }
}
