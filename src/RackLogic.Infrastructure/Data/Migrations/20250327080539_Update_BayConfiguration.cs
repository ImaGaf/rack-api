using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class Update_BayConfiguration : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.AddColumn<string>(
              name: "Code",
              table: "OEMBrands",
              type: "nvarchar(max)",
              nullable: true);

          migrationBuilder.AddColumn<Guid>(
              name: "WarehouseId",
              table: "OEMBrands",
              type: "uniqueidentifier",
              nullable: true);

          migrationBuilder.AddColumn<Guid>(
              name: "ManufacturerId",
              table: "BayConfigurations",
              type: "uniqueidentifier",
              nullable: true);

          migrationBuilder.CreateIndex(
              name: "IX_OEMBrands_WarehouseId",
              table: "OEMBrands",
              column: "WarehouseId");

          migrationBuilder.CreateIndex(
              name: "IX_BayConfigurations_ManufacturerId",
              table: "BayConfigurations",
              column: "ManufacturerId");

          migrationBuilder.AddForeignKey(
              name: "FK_BayConfigurations_OEMBrands_ManufacturerId",
              table: "BayConfigurations",
              column: "ManufacturerId",
              principalTable: "OEMBrands",
              principalColumn: "Id",
              onDelete: ReferentialAction.Cascade);

          migrationBuilder.AddForeignKey(
              name: "FK_OEMBrands_Warehouses_WarehouseId",
              table: "OEMBrands",
              column: "WarehouseId",
              principalTable: "Warehouses",
              principalColumn: "Id");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropForeignKey(
              name: "FK_BayConfigurations_OEMBrands_ManufacturerId",
              table: "BayConfigurations");

          migrationBuilder.DropForeignKey(
              name: "FK_OEMBrands_Warehouses_WarehouseId",
              table: "OEMBrands");

          migrationBuilder.DropIndex(
              name: "IX_OEMBrands_WarehouseId",
              table: "OEMBrands");

          migrationBuilder.DropIndex(
              name: "IX_BayConfigurations_ManufacturerId",
              table: "BayConfigurations");

          migrationBuilder.DropColumn(
              name: "Code",
              table: "OEMBrands");

          migrationBuilder.DropColumn(
              name: "WarehouseId",
              table: "OEMBrands");

          migrationBuilder.DropColumn(
              name: "ManufacturerId",
              table: "BayConfigurations");
      }
  }
