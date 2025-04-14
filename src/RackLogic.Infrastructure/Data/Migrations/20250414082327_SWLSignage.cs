using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class SWLSignage : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.AddColumn<Guid>(
              name: "SWLSignageId",
              table: "BayConfigurations",
              type: "uniqueidentifier",
              nullable: false,
              defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

          migrationBuilder.CreateTable(
              name: "SWLSignageEntries",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  BayConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  PalletWeight = table.Column<float>(type: "real", nullable: true),
                  BulkPalletsNumberOfLevels = table.Column<int>(type: "int", nullable: true),
                  BulkPalletsLevelMaximumLoad = table.Column<float>(type: "real", nullable: true),
                  ShelvingNumberOfLevel = table.Column<int>(type: "int", nullable: true),
                  ShelvingLevelMaximumLoad = table.Column<float>(type: "real", nullable: true),
                  ToTalBayLoad = table.Column<float>(type: "real", nullable: true),
                  FirstBeamLevelMeasurementFGL = table.Column<float>(type: "real", nullable: true),
                  SecondBeamLevelMeasurementFGL = table.Column<float>(type: "real", nullable: true),
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
                  table.PrimaryKey("PK_SWLSignageEntries", x => x.Id);
                  table.ForeignKey(
                      name: "FK_SWLSignageEntries_BayConfigurations_BayConfigurationId",
                      column: x => x.BayConfigurationId,
                      principalTable: "BayConfigurations",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateIndex(
              name: "IX_SWLSignageEntries_BayConfigurationId",
              table: "SWLSignageEntries",
              column: "BayConfigurationId",
              unique: true);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "SWLSignageEntries");

          migrationBuilder.DropColumn(
              name: "SWLSignageId",
              table: "BayConfigurations");
      }
  }
