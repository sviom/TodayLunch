using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LunchLibrary.Migrations
{
    public partial class OwnerUpdateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Owner",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Owner");
        }
    }
}
