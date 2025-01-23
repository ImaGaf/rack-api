using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class CreateWHFinalUseTable : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.CreateTable(
              name: "WarehouseFinalUses",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                  table.PrimaryKey("PK_WarehouseFinalUses", x => x.Id);
                  table.ForeignKey(
                      name: "FK_WarehouseFinalUses_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  table.ForeignKey(
                      name: "FK_WarehouseFinalUses_Warehouses_WarehouseId",
                      column: x => x.WarehouseId,
                      principalTable: "Warehouses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateIndex(
              name: "IX_WarehouseFinalUses_UserId",
              table: "WarehouseFinalUses",
              column: "UserId");

          migrationBuilder.CreateIndex(
              name: "IX_WarehouseFinalUses_WarehouseId",
              table: "WarehouseFinalUses",
              column: "WarehouseId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "WarehouseFinalUses");
      }
  }
