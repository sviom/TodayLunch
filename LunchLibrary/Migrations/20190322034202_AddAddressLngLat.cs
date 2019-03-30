using Microsoft.EntityFrameworkCore.Migrations;

namespace LunchLibrary.Migrations
{
    public partial class AddAddressLngLat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "Address",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lng",
                table: "Address",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Address");
        }
    }
}
