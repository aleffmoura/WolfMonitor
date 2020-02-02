using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitoring
{
    public partial class FirstMigrationWolfMonitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 250, nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    Default = table.Column<string>(nullable: true),
                    Value = table.Column<string>(maxLength: 250, nullable: false),
                    LastValue = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UserIdWhoAdd = table.Column<Guid>(nullable: false),
                    AgentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_AgentId",
                table: "Items",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserIdWhoAdd",
                table: "Items",
                column: "UserIdWhoAdd");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
