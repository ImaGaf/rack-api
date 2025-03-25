using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class CreateDefaultSettingsFloorPlan : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.AddColumn<Guid>(
              name: "BayConfigurationId",
              table: "FloorPlanItems",
              type: "uniqueidentifier",
              nullable: true);

          migrationBuilder.CreateTable(
              name: "DefaultSettings",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  FloorPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  BayConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                  BayConfigCategory = table.Column<int>(type: "int", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_DefaultSettings", x => x.Id);
                  table.ForeignKey(
                      name: "FK_DefaultSettings_BayConfigurations_BayConfigurationId",
                      column: x => x.BayConfigurationId,
                      principalTable: "BayConfigurations",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  table.ForeignKey(
                      name: "FK_DefaultSettings_FloorPlans_FloorPlanId",
                      column: x => x.FloorPlanId,
                      principalTable: "FloorPlans",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateIndex(
              name: "IX_DefaultSettings_BayConfigurationId",
              table: "DefaultSettings",
              column: "BayConfigurationId");

          migrationBuilder.CreateIndex(
              name: "IX_DefaultSettings_FloorPlanId",
              table: "DefaultSettings",
              column: "FloorPlanId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "DefaultSettings");

          migrationBuilder.DropColumn(
              name: "BayConfigurationId",
              table: "FloorPlanItems");
      }
  }
