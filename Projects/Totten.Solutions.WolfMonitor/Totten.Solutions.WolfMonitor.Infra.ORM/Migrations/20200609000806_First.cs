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
                    { new Guid("6573f762-eb5f-4ce9-bf92-23b61ea69cd2"), new DateTime(2020, 6, 8, 21, 8, 5, 356, DateTimeKind.Local).AddTicks(871), 0, "Agent", false, new DateTime(2020, 6, 8, 21, 8, 5, 356, DateTimeKind.Local).AddTicks(7076) },
                    { new Guid("7f31dd95-ddb0-41f1-b709-e26b7087151c"), new DateTime(2020, 6, 8, 21, 8, 5, 358, DateTimeKind.Local).AddTicks(4053), 1, "User", false, new DateTime(2020, 6, 8, 21, 8, 5, 358, DateTimeKind.Local).AddTicks(4069) },
                    { new Guid("f91a2366-c469-412a-9197-976a90516272"), new DateTime(2020, 6, 8, 21, 8, 5, 358, DateTimeKind.Local).AddTicks(4169), 2, "Admin", false, new DateTime(2020, 6, 8, 21, 8, 5, 358, DateTimeKind.Local).AddTicks(4170) },
                    { new Guid("20c02fa8-19df-4e01-b436-3219c5cc6ca8"), new DateTime(2020, 6, 8, 21, 8, 5, 358, DateTimeKind.Local).AddTicks(4207), 3, "System", false, new DateTime(2020, 6, 8, 21, 8, 5, 358, DateTimeKind.Local).AddTicks(4207) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastLogin", "LastName", "Login", "Password", "RecoverSolicitationCode", "Removed", "RoleId", "Token", "TokenSolicitationCode", "UpdatedIn" },
                values: new object[] { new Guid("f75a1881-0fd6-4273-9d23-c59018788201"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "11111111111", new DateTime(2020, 6, 8, 21, 8, 5, 377, DateTimeKind.Local).AddTicks(87), "aleffmds@gmail.com", "Aleff", "pt-BR", null, "Moura da Silva", "aleffmoura", "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", null, false, new Guid("20c02fa8-19df-4e01-b436-3219c5cc6ca8"), null, null, new DateTime(2020, 6, 8, 21, 8, 5, 377, DateTimeKind.Local).AddTicks(648) });

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
