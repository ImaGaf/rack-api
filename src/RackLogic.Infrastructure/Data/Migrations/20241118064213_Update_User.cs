using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackLogic.Infrastructure.Data.Migrations;

  /// <inheritdoc />
  public partial class Update_User : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.AddColumn<DateTime>(
              name: "LockedUntil",
              table: "AspNetUsers",
              type: "datetime2",
              nullable: true);

          migrationBuilder.AddColumn<int>(
              name: "OtpSendCount",
              table: "AspNetUsers",
              type: "int",
              nullable: false,
              defaultValue: 0);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropColumn(
              name: "LockedUntil",
              table: "AspNetUsers");

          migrationBuilder.DropColumn(
              name: "OtpSendCount",
              table: "AspNetUsers");
      }
  }
