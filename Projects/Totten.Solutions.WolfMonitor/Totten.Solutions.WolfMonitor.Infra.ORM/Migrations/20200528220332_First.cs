using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations
{
    public partial class First : Migration
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
                    Language = table.Column<string>(nullable: false),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    TokenSolicitationCode = table.Column<string>(nullable: true),
                    RecoverSolicitationCode = table.Column<string>(nullable: true)
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
                    { new Guid("0b17ce07-c0f0-4d30-9a28-e82ff50d0e58"), new DateTime(2020, 5, 28, 19, 3, 31, 637, DateTimeKind.Local).AddTicks(6769), 0, "Agent", false, new DateTime(2020, 5, 28, 19, 3, 31, 638, DateTimeKind.Local).AddTicks(3999) },
                    { new Guid("435b9712-c523-4118-86f0-095cae7f3336"), new DateTime(2020, 5, 28, 19, 3, 31, 640, DateTimeKind.Local).AddTicks(804), 1, "User", false, new DateTime(2020, 5, 28, 19, 3, 31, 640, DateTimeKind.Local).AddTicks(810) },
                    { new Guid("f91a2366-c469-412a-9197-976a90516272"), new DateTime(2020, 5, 28, 19, 3, 31, 640, DateTimeKind.Local).AddTicks(933), 2, "Admin", false, new DateTime(2020, 5, 28, 19, 3, 31, 640, DateTimeKind.Local).AddTicks(934) },
                    { new Guid("c7d3fdb8-be1e-4b3c-aa49-729dcb12f241"), new DateTime(2020, 5, 28, 19, 3, 31, 640, DateTimeKind.Local).AddTicks(974), 3, "System", false, new DateTime(2020, 5, 28, 19, 3, 31, 640, DateTimeKind.Local).AddTicks(974) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastLogin", "LastName", "Login", "Password", "RecoverSolicitationCode", "Removed", "RoleId", "Token", "TokenSolicitationCode", "UpdatedIn" },
                values: new object[] { new Guid("f75a1881-0fd6-4273-9d23-c59018788201"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "11111111111", new DateTime(2020, 5, 28, 19, 3, 31, 658, DateTimeKind.Local).AddTicks(9313), "aleffmds@gmail.com", "Aleff", "pt-BR", null, "Moura da Silva", "aleffmoura", "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", null, false, new Guid("c7d3fdb8-be1e-4b3c-aa49-729dcb12f241"), null, null, new DateTime(2020, 5, 28, 19, 3, 31, 658, DateTimeKind.Local).AddTicks(9848) });

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
