using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class update_WareHouse : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropForeignKey(
              name: "FK_UserWarehouseRoles_IdentityRole_RoleId1",
              table: "UserWarehouseRoles");

          migrationBuilder.DropTable(
              name: "IdentityRole");

          migrationBuilder.DropIndex(
              name: "IX_UserWarehouseRoles_RoleId1",
              table: "UserWarehouseRoles");

          migrationBuilder.DropColumn(
              name: "RoleId1",
              table: "UserWarehouseRoles");

          migrationBuilder.CreateIndex(
              name: "IX_UserWarehouseRoles_RoleId",
              table: "UserWarehouseRoles",
              column: "RoleId");

          migrationBuilder.AddForeignKey(
              name: "FK_UserWarehouseRoles_AspNetRoles_RoleId",
              table: "UserWarehouseRoles",
              column: "RoleId",
              principalTable: "AspNetRoles",
              principalColumn: "Id",
              onDelete: ReferentialAction.Cascade);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropForeignKey(
              name: "FK_UserWarehouseRoles_AspNetRoles_RoleId",
              table: "UserWarehouseRoles");

          migrationBuilder.DropIndex(
              name: "IX_UserWarehouseRoles_RoleId",
              table: "UserWarehouseRoles");

          migrationBuilder.AddColumn<string>(
              name: "RoleId1",
              table: "UserWarehouseRoles",
              type: "nvarchar(450)",
              nullable: true);

          migrationBuilder.CreateTable(
              name: "IdentityRole",
              columns: table => new
              {
                  Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                  ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_IdentityRole", x => x.Id);
              });

          migrationBuilder.CreateIndex(
              name: "IX_UserWarehouseRoles_RoleId1",
              table: "UserWarehouseRoles",
              column: "RoleId1");

          migrationBuilder.AddForeignKey(
              name: "FK_UserWarehouseRoles_IdentityRole_RoleId1",
              table: "UserWarehouseRoles",
              column: "RoleId1",
              principalTable: "IdentityRole",
              principalColumn: "Id");
      }
  }
