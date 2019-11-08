using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.Auth
{
    public partial class FirstMigrationCreatedUsersRoles : Migration
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
                    { new Guid("71b178bf-e972-4c55-837e-ec9d3e45806f"), new DateTime(2019, 11, 8, 17, 56, 5, 207, DateTimeKind.Local).AddTicks(4775), 0, "Agent", false, new DateTime(2019, 11, 8, 17, 56, 5, 213, DateTimeKind.Local).AddTicks(2463) },
                    { new Guid("599a14e9-02c1-47ae-9046-8cf3771a7044"), new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8631), 1, "User", false, new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8690) },
                    { new Guid("6d936a6b-1102-4838-8ef9-c452e6451861"), new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8827), 2, "Administrador", false, new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8846) },
                    { new Guid("93c434db-c88a-41cc-be95-aa57838bc8ec"), new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8895), 2, "System", false, new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8910) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastName", "Login", "Password", "Removed", "RoleId", "UpdatedIn" },
                values: new object[] { new Guid("403f5ecd-5f98-434e-8a01-3109e7908b76"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "10685805425", new DateTime(2019, 11, 8, 17, 56, 5, 254, DateTimeKind.Local).AddTicks(4386), "aleffmds@gmail.com", "Aleff", "pt-BR", "Moura da Silva", "alefmoura", "YWLA/fjq/N2i5CmP6+HJMXDslpQwLLP2tHl4E9NaM8w=", false, new Guid("93c434db-c88a-41cc-be95-aa57838bc8ec"), new DateTime(2019, 11, 8, 17, 56, 5, 254, DateTimeKind.Local).AddTicks(4450) });

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
