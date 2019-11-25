using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitoring
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(nullable: true),
                    UserIdWhoAdd = table.Column<Guid>(nullable: false),
                    AgentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemServices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemServices_AgentId",
                table: "SystemServices",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemServices_UserIdWhoAdd",
                table: "SystemServices",
                column: "UserIdWhoAdd");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemServices");
        }
    }
}
