using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HdProduction.Dashboard.Infrastructure.Migrations
{
    public partial class ProjectBuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "project_build",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ProjectId = table.Column<long>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SelfHostConfiguration = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 0),
                    LinkToDownload = table.Column<string>(maxLength: 512, nullable: true),
                    Error = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 2, 25, 21, 35, 55, 535, DateTimeKind.Utc).AddTicks(390))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_build", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_build_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_project_build_ProjectId",
                table: "project_build",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project_build");
        }
    }
}
