using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class WareHouse : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.CreateTable(
              name: "IdentityRole",
              columns: table => new
              {
                  Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                  Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_IdentityRole", x => x.Id);
              });

          migrationBuilder.CreateTable(
              name: "Warehouses",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Warehouses", x => x.Id);
              });

          migrationBuilder.CreateTable(
              name: "UserWarehouseRoles",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  WareHoseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  RoleId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                  table.PrimaryKey("PK_UserWarehouseRoles", x => x.Id);
                  table.ForeignKey(
                      name: "FK_UserWarehouseRoles_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  table.ForeignKey(
                      name: "FK_UserWarehouseRoles_IdentityRole_RoleId1",
                      column: x => x.RoleId1,
                      principalTable: "IdentityRole",
                      principalColumn: "Id");
                  table.ForeignKey(
                      name: "FK_UserWarehouseRoles_Warehouses_WarehouseId",
                      column: x => x.WarehouseId,
                      principalTable: "Warehouses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateIndex(
              name: "IX_UserWarehouseRoles_RoleId1",
              table: "UserWarehouseRoles",
              column: "RoleId1");

          migrationBuilder.CreateIndex(
              name: "IX_UserWarehouseRoles_UserId",
              table: "UserWarehouseRoles",
              column: "UserId");

          migrationBuilder.CreateIndex(
              name: "IX_UserWarehouseRoles_WarehouseId",
              table: "UserWarehouseRoles",
              column: "WarehouseId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "UserWarehouseRoles");

          migrationBuilder.DropTable(
              name: "IdentityRole");

          migrationBuilder.DropTable(
              name: "Warehouses");
      }
  }
