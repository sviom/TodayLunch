using Microsoft.EntityFrameworkCore.Migrations;

namespace LunchLibrary.Migrations
{
    public partial class PlaceOwner_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Place_Owner_OwnerId",
                table: "Place");

            migrationBuilder.DropIndex(
                name: "IX_Place_OwnerId",
                table: "Place");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Place_OwnerId",
                table: "Place",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Place_Owner_OwnerId",
                table: "Place",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
