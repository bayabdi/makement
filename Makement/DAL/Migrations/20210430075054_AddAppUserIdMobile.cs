using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddAppUserIdMobile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AppInfoMobiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppInfoMobiles_UserId",
                table: "AppInfoMobiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppInfoMobiles_AspNetUsers_UserId",
                table: "AppInfoMobiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppInfoMobiles_AspNetUsers_UserId",
                table: "AppInfoMobiles");

            migrationBuilder.DropIndex(
                name: "IX_AppInfoMobiles_UserId",
                table: "AppInfoMobiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AppInfoMobiles");
        }
    }
}
