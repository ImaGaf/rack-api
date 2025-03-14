using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class table_BayConfiguration : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.CreateTable(
              name: "BayConfigurations",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  WarahouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                  IsCustomBrand = table.Column<bool>(type: "bit", nullable: false),
                  BayConfigId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                  RackingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  InstallationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                  IsEstimatedInstallDate = table.Column<bool>(type: "bit", nullable: false),
                  ASComplianceVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Supplier = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                  StructuralDesigner = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                  ProjectRef = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                  ToleranceGrade = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                  TotalBayLoad = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                  Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  IsEnableInterimInspection = table.Column<bool>(type: "bit", nullable: false),
                  InterimInterval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  HasSplice = table.Column<bool>(type: "bit", nullable: false),
                  Level = table.Column<int>(type: "int", nullable: false),
                  Pallets = table.Column<int>(type: "int", nullable: false),
                  HasProtector = table.Column<bool>(type: "bit", nullable: false),
                  SpliceKits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  IsCustomUpRight = table.Column<bool>(type: "bit", nullable: true),
                  IsCustomBeam = table.Column<bool>(type: "bit", nullable: true),
                  IsCustomBracingHorizontal = table.Column<bool>(type: "bit", nullable: true),
                  IsCustomBracingDiagonal = table.Column<bool>(type: "bit", nullable: true),
                  IsCustomRowSpacer = table.Column<bool>(type: "bit", nullable: true),
                  IsCustomAccessories = table.Column<bool>(type: "bit", nullable: true),
                  RowSpacerLevel = table.Column<int>(type: "int", nullable: false),
                  BracingDiagonalLevel = table.Column<int>(type: "int", nullable: false),
                  BracingLevel = table.Column<int>(type: "int", nullable: false),
                  BeamLevel = table.Column<int>(type: "int", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_BayConfigurations", x => x.Id);
                  table.ForeignKey(
                      name: "FK_BayConfigurations_Warehouses_WarahouseId",
                      column: x => x.WarahouseId,
                      principalTable: "Warehouses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "BayConfigurationLevels",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  BayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  LevelType = table.Column<int>(type: "int", nullable: false),
                  LevelValue = table.Column<int>(type: "int", nullable: true),
                  Orientation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  LowerHeight = table.Column<int>(type: "int", nullable: true),
                  UpperHeight = table.Column<int>(type: "int", nullable: true),
                  IsXBrace = table.Column<bool>(type: "bit", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_BayConfigurationLevels", x => x.Id);
                  table.ForeignKey(
                      name: "FK_BayConfigurationLevels_BayConfigurations_BayId",
                      column: x => x.BayId,
                      principalTable: "BayConfigurations",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "Components",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  BayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  ComponentType = table.Column<int>(type: "int", nullable: false),
                  Height = table.Column<int>(type: "int", nullable: true),
                  Width = table.Column<int>(type: "int", nullable: true),
                  Length = table.Column<int>(type: "int", nullable: true),
                  Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  Orientation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  DistanceCenterToCenter = table.Column<int>(type: "int", nullable: true),
                  Category = table.Column<int>(type: "int", nullable: true),
                  Quantity = table.Column<int>(type: "int", nullable: true),
                  Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  LowerHeight = table.Column<int>(type: "int", nullable: true),
                  UpperHeight = table.Column<int>(type: "int", nullable: true),
                  IsXBrace = table.Column<bool>(type: "bit", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Components", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Components_BayConfigurations_BayId",
                      column: x => x.BayId,
                      principalTable: "BayConfigurations",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateIndex(
              name: "IX_BayConfigurationLevels_BayId",
              table: "BayConfigurationLevels",
              column: "BayId");

          migrationBuilder.CreateIndex(
              name: "IX_BayConfigurations_WarahouseId",
              table: "BayConfigurations",
              column: "WarahouseId");

          migrationBuilder.CreateIndex(
              name: "IX_Components_BayId",
              table: "Components",
              column: "BayId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "BayConfigurationLevels");

          migrationBuilder.DropTable(
              name: "Components");

          migrationBuilder.DropTable(
              name: "BayConfigurations");
      }
  }
