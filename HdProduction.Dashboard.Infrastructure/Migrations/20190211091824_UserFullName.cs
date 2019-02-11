using Microsoft.EntityFrameworkCore.Migrations;

namespace HdProduction.Dashboard.Infrastructure.Migrations
{
    public partial class UserFullName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "user",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "user",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "user");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "user");
        }
    }
}
