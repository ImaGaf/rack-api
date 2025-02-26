using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Table_PurchaseRecode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoRenewing = table.Column<bool>(type: "bit", nullable: false),
                    IsRefunded = table.Column<bool>(type: "bit", nullable: false),
                    ReceiptData = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_PurchaseRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRecords_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRecords_UserId",
                table: "PurchaseRecords",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseRecords");
        }
    }
}
