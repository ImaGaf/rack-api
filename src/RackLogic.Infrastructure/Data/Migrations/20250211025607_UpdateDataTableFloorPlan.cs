using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class UpdateDataTableFloorPlan : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropColumn(
              name: "Floor",
              table: "FloorPlans");

          migrationBuilder.AddColumn<string>(
              name: "Description",
              table: "FloorPlans",
              type: "nvarchar(max)",
              nullable: true);

          migrationBuilder.AddColumn<int>(
              name: "FloorNumber",
              table: "FloorPlans",
              type: "int",
              nullable: false,
              defaultValue: 0);

          migrationBuilder.AddColumn<int>(
              name: "totalBay",
              table: "FloorPlans",
              type: "int",
              nullable: true);

          migrationBuilder.AddColumn<int>(
              name: "totalRack",
              table: "FloorPlans",
              type: "int",
              nullable: true);

          migrationBuilder.CreateTable(
              name: "FloorPlanItems",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Rack = table.Column<int>(type: "int", nullable: false),
                  Bay = table.Column<int>(type: "int", nullable: false),
                  FloorPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Offset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Size = table.Column<int>(type: "int", nullable: true),
                  Width = table.Column<double>(type: "float", nullable: true),
                  Height = table.Column<double>(type: "float", nullable: true),
                  ObjectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Rotation = table.Column<double>(type: "float", nullable: false),
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
                  table.PrimaryKey("PK_FloorPlanItems", x => x.Id);
                  table.ForeignKey(
                      name: "FK_FloorPlanItems_FloorPlans_FloorPlanId",
                      column: x => x.FloorPlanId,
                      principalTable: "FloorPlans",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "FloorPlanComments",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  FloorPlanItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                  table.PrimaryKey("PK_FloorPlanComments", x => x.Id);
                  table.ForeignKey(
                      name: "FK_FloorPlanComments_FloorPlanItems_FloorPlanItemId",
                      column: x => x.FloorPlanItemId,
                      principalTable: "FloorPlanItems",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateIndex(
              name: "IX_FloorPlanComments_FloorPlanItemId",
              table: "FloorPlanComments",
              column: "FloorPlanItemId");

          migrationBuilder.CreateIndex(
              name: "IX_FloorPlanItems_FloorPlanId",
              table: "FloorPlanItems",
              column: "FloorPlanId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "FloorPlanComments");

          migrationBuilder.DropTable(
              name: "FloorPlanItems");

          migrationBuilder.DropColumn(
              name: "Description",
              table: "FloorPlans");

          migrationBuilder.DropColumn(
              name: "FloorNumber",
              table: "FloorPlans");

          migrationBuilder.DropColumn(
              name: "totalBay",
              table: "FloorPlans");

          migrationBuilder.DropColumn(
              name: "totalRack",
              table: "FloorPlans");

          migrationBuilder.AddColumn<string>(
              name: "Floor",
              table: "FloorPlans",
              type: "nvarchar(max)",
              nullable: false,
              defaultValue: "");
      }
  }
