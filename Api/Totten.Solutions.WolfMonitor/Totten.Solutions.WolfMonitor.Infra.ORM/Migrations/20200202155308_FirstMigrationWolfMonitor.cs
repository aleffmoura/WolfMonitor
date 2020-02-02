using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations
{
    public partial class FirstMigrationWolfMonitor : Migration
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
                    UserWhoCreatedName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: false),
                    MachineName = table.Column<string>(nullable: true),
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
                values: new object[] { new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "35.344.681/0001-90", new DateTime(2020, 2, 2, 12, 53, 7, 844, DateTimeKind.Local).AddTicks(9898), "tottemsolutions", "ALEFF MOURA DA SILVA 10685805425", false, new DateTime(2020, 2, 2, 12, 53, 7, 846, DateTimeKind.Local).AddTicks(3793) });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "CompanyId", "Configured", "CreatedIn", "DisplayName", "FirstConnection", "HostAddress", "HostName", "LastConnection", "LastUpload", "LocalIp", "Login", "MachineName", "Password", "Removed", "UpdatedIn", "UserWhoCreatedId", "UserWhoCreatedName" },
                values: new object[] { new Guid("56159a2a-bbfd-413a-be9c-445bd8bbe894"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), false, new DateTime(2020, 2, 2, 12, 53, 7, 848, DateTimeKind.Local).AddTicks(5113), "Servidor BR 1", null, null, null, null, null, null, "servidor1", null, "123456", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f91a2366-c469-412a-9197-976a90516272"), "Admin" });

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
