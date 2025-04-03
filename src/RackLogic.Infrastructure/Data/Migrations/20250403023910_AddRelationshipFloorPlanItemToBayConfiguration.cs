using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipFloorPlanItemToBayConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FloorPlanItems_BayConfigurationId",
                table: "FloorPlanItems",
                column: "BayConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorPlanItems_BayConfigurations_BayConfigurationId",
                table: "FloorPlanItems",
                column: "BayConfigurationId",
                principalTable: "BayConfigurations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorPlanItems_BayConfigurations_BayConfigurationId",
                table: "FloorPlanItems");

            migrationBuilder.DropIndex(
                name: "IX_FloorPlanItems_BayConfigurationId",
                table: "FloorPlanItems");
        }
    }
}
