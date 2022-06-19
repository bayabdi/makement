using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class DeleteTeamRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTeams_TeamRoles_RoleId",
                table: "UserTeams");

            migrationBuilder.DropTable(
                name: "TeamRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserTeams_RoleId",
                table: "UserTeams");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserTeams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "UserTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TeamRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRoles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TeamRoles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Manager" });

            migrationBuilder.InsertData(
                table: "TeamRoles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Employee" });

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_RoleId",
                table: "UserTeams",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeams_TeamRoles_RoleId",
                table: "UserTeams",
                column: "RoleId",
                principalTable: "TeamRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
