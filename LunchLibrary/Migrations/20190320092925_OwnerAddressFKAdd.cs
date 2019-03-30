using Microsoft.EntityFrameworkCore.Migrations;

namespace LunchLibrary.Migrations
{
    public partial class OwnerAddressFKAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Address_OwnerId",
                table: "Address",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Owner_OwnerId",
                table: "Address",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Owner_OwnerId",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_OwnerId",
                table: "Address");
        }
    }
}
