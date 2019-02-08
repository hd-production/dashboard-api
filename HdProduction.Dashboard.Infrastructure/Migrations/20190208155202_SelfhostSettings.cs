using Microsoft.EntityFrameworkCore.Migrations;

namespace HdProduction.Dashboard.Infrastructure.Migrations
{
    public partial class SelfhostSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "user",
                fixedLength: true,
                maxLength: 44,
                nullable: true,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelfHostSettings",
                table: "project",
                type: "Json",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelfHostSettings",
                table: "project");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "user",
                fixedLength: true,
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 44,
                oldNullable: true);
        }
    }
}
