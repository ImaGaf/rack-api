using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class addUserOtp : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.CreateTable(
              name: "UserOtps",
              columns: table => new
              {
                  Id = table.Column<int>(type: "int", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),
                  UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Otp = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                  table.PrimaryKey("PK_UserOtps", x => x.Id);
                  table.ForeignKey(
                      name: "FK_UserOtps_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateIndex(
              name: "IX_UserOtps_UserId",
              table: "UserOtps",
              column: "UserId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "UserOtps");
      }
  }
