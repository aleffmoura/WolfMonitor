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
                    { new Guid("020894b7-53f1-468a-9695-a2b6c47406ce"), new DateTime(2019, 11, 8, 20, 27, 37, 456, DateTimeKind.Local).AddTicks(6569), 0, "Agent", false, new DateTime(2019, 11, 8, 20, 27, 37, 463, DateTimeKind.Local).AddTicks(590) },
                    { new Guid("4ef84fad-8dee-4395-bae1-65fa8fa11dcc"), new DateTime(2019, 11, 8, 20, 27, 37, 465, DateTimeKind.Local).AddTicks(9185), 1, "User", false, new DateTime(2019, 11, 8, 20, 27, 37, 465, DateTimeKind.Local).AddTicks(9229) },
                    { new Guid("d300b446-2fd4-4059-a074-6af0ba419cea"), new DateTime(2019, 11, 8, 20, 27, 37, 465, DateTimeKind.Local).AddTicks(9410), 2, "Administrador", false, new DateTime(2019, 11, 8, 20, 27, 37, 465, DateTimeKind.Local).AddTicks(9429) },
                    { new Guid("05f62ca8-462f-4796-bab1-ed083cebf096"), new DateTime(2019, 11, 8, 20, 27, 37, 465, DateTimeKind.Local).AddTicks(9478), 2, "System", false, new DateTime(2019, 11, 8, 20, 27, 37, 465, DateTimeKind.Local).AddTicks(9498) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastName", "Login", "Password", "Removed", "RoleId", "UpdatedIn" },
                values: new object[] { new Guid("e0704212-13e5-4809-9b45-5d1f222eceec"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "10685805425", new DateTime(2019, 11, 8, 20, 27, 37, 492, DateTimeKind.Local).AddTicks(7594), "aleffmds@gmail.com", "Aleff", "pt-BR", "Moura da Silva", "alefmoura", "YWLA/fjq/N2i5CmP6+HJMXDslpQwLLP2tHl4E9NaM8w=", false, new Guid("05f62ca8-462f-4796-bab1-ed083cebf096"), new DateTime(2019, 11, 8, 20, 27, 37, 492, DateTimeKind.Local).AddTicks(7663) });

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
