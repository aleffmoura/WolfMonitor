using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.Auth
{
    public partial class FirstMigrationWolfMonitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedIn", "Level", "Name", "Removed", "UpdatedIn" },
                values: new object[,]
                {
                    { new Guid("6a5ecc23-e4ea-4077-ac04-6836e188ab0b"), new DateTime(2020, 2, 2, 12, 54, 2, 704, DateTimeKind.Local).AddTicks(4752), 0, "Agent", false, new DateTime(2020, 2, 2, 12, 54, 2, 705, DateTimeKind.Local).AddTicks(4502) },
                    { new Guid("527d5006-c3bf-4d97-8c7d-387a967894fe"), new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8287), 1, "User", false, new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8306) },
                    { new Guid("f91a2366-c469-412a-9197-976a90516272"), new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8447), 2, "Admin", false, new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8449) },
                    { new Guid("93409973-4e84-44f8-8997-b452921dba34"), new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8539), 3, "System", false, new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8541) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastName", "Login", "Password", "Removed", "RoleId", "UpdatedIn" },
                values: new object[] { new Guid("ccc8b552-9b77-4318-88ba-56beeea131fe"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "10685805425", new DateTime(2020, 2, 2, 12, 54, 2, 723, DateTimeKind.Local).AddTicks(193), "aleffmds@gmail.com", "Aleff", "pt-BR", "Moura da Silva", "aleffmoura", "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", false, new Guid("93409973-4e84-44f8-8997-b452921dba34"), new DateTime(2020, 2, 2, 12, 54, 2, 723, DateTimeKind.Local).AddTicks(230) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
