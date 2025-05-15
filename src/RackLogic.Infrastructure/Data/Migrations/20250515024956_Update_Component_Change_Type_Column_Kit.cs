using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Component_Change_Type_Column_Kit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Kit",
                table: "Components",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_WarehouseId",
                table: "Inspections",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Warehouses_WarehouseId",
                table: "Inspections",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Warehouses_WarehouseId",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_WarehouseId",
                table: "Inspections");

            migrationBuilder.AlterColumn<int>(
                name: "Kit",
                table: "Components",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
