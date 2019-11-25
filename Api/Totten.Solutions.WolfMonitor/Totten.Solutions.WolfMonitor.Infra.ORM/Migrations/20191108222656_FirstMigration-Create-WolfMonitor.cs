using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations
{
    public partial class FirstMigrationCreateWolfMonitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    FantasyName = table.Column<string>(nullable: false),
                    Cnpj = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    UserWhoCreatedId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocalIp = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    HostAddress = table.Column<string>(nullable: true),
                    Login = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    Configured = table.Column<bool>(nullable: false),
                    FirstConnection = table.Column<DateTime>(nullable: true),
                    LastConnection = table.Column<DateTime>(nullable: true),
                    LastUpload = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agents_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Cnpj", "CreatedIn", "FantasyName", "Name", "Removed", "UpdatedIn" },
                values: new object[] { new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "35.344.681/0001-90", new DateTime(2019, 11, 8, 20, 26, 55, 547, DateTimeKind.Local).AddTicks(2676), "TOTEM SOLUTIONS", "ALEFF MOURA DA SILVA 10685805425", false, new DateTime(2019, 11, 8, 20, 26, 55, 549, DateTimeKind.Local).AddTicks(9003) });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_CompanyId",
                table: "Agents",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_UserWhoCreatedId",
                table: "Agents",
                column: "UserWhoCreatedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
