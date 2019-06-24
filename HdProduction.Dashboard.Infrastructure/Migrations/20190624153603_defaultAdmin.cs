using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HdProduction.Dashboard.Infrastructure.Migrations
{
    public partial class defaultAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdate",
                table: "project_build",
                nullable: false,
                defaultValue: new DateTime(2019, 6, 24, 15, 36, 3, 486, DateTimeKind.Utc).AddTicks(5845),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 2, 25, 21, 35, 55, 535, DateTimeKind.Utc).AddTicks(390));

            migrationBuilder.AddColumn<string>(
                name: "DefaultAdminSettings",
                table: "project",
                type: "Json",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "project",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultAdminSettings",
                table: "project");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "project");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdate",
                table: "project_build",
                nullable: false,
                defaultValue: new DateTime(2019, 2, 25, 21, 35, 55, 535, DateTimeKind.Utc).AddTicks(390),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 6, 24, 15, 36, 3, 486, DateTimeKind.Utc).AddTicks(5845));
        }
    }
}
