using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefectLogHistoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLog_Components_ComponentId",
                table: "DefectLog");

            migrationBuilder.AddColumn<int>(
                name: "DefectStatus",
                table: "DefectLog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DefectLogHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefectLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_DefectLogHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefectLogHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefectLogHistory_DefectLog_DefectLogId",
                        column: x => x.DefectLogId,
                        principalTable: "DefectLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefectLogHistory_DefectLogId",
                table: "DefectLogHistory",
                column: "DefectLogId");

            migrationBuilder.CreateIndex(
                name: "IX_DefectLogHistory_UserId",
                table: "DefectLogHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLog_Components_ComponentId",
                table: "DefectLog",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectLog_Components_ComponentId",
                table: "DefectLog");

            migrationBuilder.DropTable(
                name: "DefectLogHistory");

            migrationBuilder.DropColumn(
                name: "DefectStatus",
                table: "DefectLog");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectLog_Components_ComponentId",
                table: "DefectLog",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
