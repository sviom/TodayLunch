using Microsoft.EntityFrameworkCore.Migrations;

namespace LunchLibrary.Migrations
{
    public partial class AddressPlaceFKAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Place_AddressId",
                table: "Place",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Place_Address_AddressId",
                table: "Place",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Place_Address_AddressId",
                table: "Place");

            migrationBuilder.DropIndex(
                name: "IX_Place_AddressId",
                table: "Place");
        }
    }
}
