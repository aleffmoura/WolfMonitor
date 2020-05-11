using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.Auth
{
    public partial class FirstMigration : Migration
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
                    { new Guid("bd81cd51-9354-4f70-a911-b4ca52392fd3"), new DateTime(2020, 4, 19, 11, 25, 3, 798, DateTimeKind.Local).AddTicks(8312), 0, "Agent", false, new DateTime(2020, 4, 19, 11, 25, 3, 799, DateTimeKind.Local).AddTicks(6306) },
                    { new Guid("3cbb8295-8138-443a-a51e-f4fc4e463a62"), new DateTime(2020, 4, 19, 11, 25, 3, 801, DateTimeKind.Local).AddTicks(6225), 1, "User", false, new DateTime(2020, 4, 19, 11, 25, 3, 801, DateTimeKind.Local).AddTicks(6239) },
                    { new Guid("f91a2366-c469-412a-9197-976a90516272"), new DateTime(2020, 4, 19, 11, 25, 3, 801, DateTimeKind.Local).AddTicks(6374), 2, "Admin", false, new DateTime(2020, 4, 19, 11, 25, 3, 801, DateTimeKind.Local).AddTicks(6376) },
                    { new Guid("0e2c36c6-2c49-4832-bebd-5a02b2fe6582"), new DateTime(2020, 4, 19, 11, 25, 3, 801, DateTimeKind.Local).AddTicks(6423), 3, "System", false, new DateTime(2020, 4, 19, 11, 25, 3, 801, DateTimeKind.Local).AddTicks(6424) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastLogin", "LastName", "Login", "Password", "RecoverSolicitationCode", "Removed", "RoleId", "Token", "TokenSolicitationCode", "UpdatedIn" },
                values: new object[] { new Guid("f75a1881-0fd6-4273-9d23-c59018788201"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "10685805425", new DateTime(2020, 4, 19, 11, 25, 3, 830, DateTimeKind.Local).AddTicks(216), "aleffmds@gmail.com", "Aleff", "pt-BR", null, "Moura da Silva", "aleffmoura", "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", null, false, new Guid("0e2c36c6-2c49-4832-bebd-5a02b2fe6582"), null, null, new DateTime(2020, 4, 19, 11, 25, 3, 830, DateTimeKind.Local).AddTicks(968) });

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
