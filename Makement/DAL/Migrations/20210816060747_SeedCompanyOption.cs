using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class SeedCompanyOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompaniesOptions",
                columns: new[] { "Id", "CompanyId", "IsTrackActivity", "IsTrackAppUsage", "IsTrackLocation", "IsTrackScreenShot" },
                values: new object[] { 1, 1, true, true, true, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompaniesOptions",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
