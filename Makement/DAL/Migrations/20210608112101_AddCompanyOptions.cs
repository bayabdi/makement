using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddCompanyOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOptions");

            migrationBuilder.CreateTable(
                name: "CompaniesOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsTrackActivity = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackAppUsage = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackAppMobileUsage = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackLocation = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackScreenShot = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompaniesOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompaniesOptions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompaniesOptions_CompanyId",
                table: "CompaniesOptions",
                column: "CompanyId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompaniesOptions");

            migrationBuilder.CreateTable(
                name: "UserOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsTrackActivity = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackAppMobileUsage = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackAppUsage = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackLocation = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackScreenShot = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOptions_UserId",
                table: "UserOptions",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
