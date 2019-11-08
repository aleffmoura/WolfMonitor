using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.Auth
{
    public partial class FirstMigrationCreateAuthContext : Migration
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
                    { new Guid("62194933-f75f-4e97-a9f0-be2afc929abd"), new DateTime(2019, 11, 8, 20, 30, 39, 896, DateTimeKind.Local).AddTicks(7323), 0, "Agent", false, new DateTime(2019, 11, 8, 20, 30, 39, 901, DateTimeKind.Local).AddTicks(8587) },
                    { new Guid("cf0a5a79-0309-4747-9da3-8b85dff2d633"), new DateTime(2019, 11, 8, 20, 30, 39, 904, DateTimeKind.Local).AddTicks(1864), 1, "User", false, new DateTime(2019, 11, 8, 20, 30, 39, 904, DateTimeKind.Local).AddTicks(1912) },
                    { new Guid("02519d3e-6aaa-488a-96a8-b79093c0fc19"), new DateTime(2019, 11, 8, 20, 30, 39, 904, DateTimeKind.Local).AddTicks(2103), 2, "Administrador", false, new DateTime(2019, 11, 8, 20, 30, 39, 904, DateTimeKind.Local).AddTicks(2123) },
                    { new Guid("f064e428-4dd6-442c-9c8d-3e1590fdef3f"), new DateTime(2019, 11, 8, 20, 30, 39, 904, DateTimeKind.Local).AddTicks(2162), 3, "System", false, new DateTime(2019, 11, 8, 20, 30, 39, 904, DateTimeKind.Local).AddTicks(2176) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastName", "Login", "Password", "Removed", "RoleId", "UpdatedIn" },
                values: new object[] { new Guid("9c93f113-76b1-4caf-9628-be844a68cb50"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "10685805425", new DateTime(2019, 11, 8, 20, 30, 39, 931, DateTimeKind.Local).AddTicks(9885), "aleffmds@gmail.com", "Aleff", "pt-BR", "Moura da Silva", "alefmoura", "YWLA/fjq/N2i5CmP6+HJMXDslpQwLLP2tHl4E9NaM8w=", false, new Guid("f064e428-4dd6-442c-9c8d-3e1590fdef3f"), new DateTime(2019, 11, 8, 20, 30, 39, 931, DateTimeKind.Local).AddTicks(9949) });

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
