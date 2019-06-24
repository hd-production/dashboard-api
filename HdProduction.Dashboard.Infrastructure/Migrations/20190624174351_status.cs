using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HdProduction.Dashboard.Infrastructure.Migrations
{
    public partial class status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdate",
                table: "project_build",
                nullable: false,
                defaultValue: new DateTime(2019, 6, 24, 17, 43, 51, 198, DateTimeKind.Utc).AddTicks(9063),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 6, 24, 15, 36, 3, 486, DateTimeKind.Utc).AddTicks(5845));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "project",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdate",
                table: "project_build",
                nullable: false,
                defaultValue: new DateTime(2019, 6, 24, 15, 36, 3, 486, DateTimeKind.Utc).AddTicks(5845),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 6, 24, 17, 43, 51, 198, DateTimeKind.Utc).AddTicks(9063));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "project",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);
        }
    }
}
