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
                    { new Guid("ea70334a-c072-4f19-8ffc-70b50e0caf4a"), new DateTime(2020, 6, 10, 19, 52, 21, 969, DateTimeKind.Local).AddTicks(3714), 0, "Agent", false, new DateTime(2020, 6, 10, 19, 52, 21, 970, DateTimeKind.Local).AddTicks(165) },
                    { new Guid("b1ab21fa-8fd9-4b23-bdbe-09cb5e9f5434"), new DateTime(2020, 6, 10, 19, 52, 21, 972, DateTimeKind.Local).AddTicks(4629), 1, "User", false, new DateTime(2020, 6, 10, 19, 52, 21, 972, DateTimeKind.Local).AddTicks(4648) },
                    { new Guid("f91a2366-c469-412a-9197-976a90516272"), new DateTime(2020, 6, 10, 19, 52, 21, 972, DateTimeKind.Local).AddTicks(4804), 2, "Admin", false, new DateTime(2020, 6, 10, 19, 52, 21, 972, DateTimeKind.Local).AddTicks(4805) },
                    { new Guid("e36d8c0c-7e31-4b7f-9c25-cf5ab0516b6c"), new DateTime(2020, 6, 10, 19, 52, 21, 972, DateTimeKind.Local).AddTicks(4851), 3, "System", false, new DateTime(2020, 6, 10, 19, 52, 21, 972, DateTimeKind.Local).AddTicks(4851) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastLogin", "LastName", "Login", "Password", "RecoverSolicitationCode", "Removed", "RoleId", "Token", "TokenSolicitationCode", "UpdatedIn" },
                values: new object[] { new Guid("f75a1881-0fd6-4273-9d23-c59018788201"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "11111111111", new DateTime(2020, 6, 10, 19, 52, 21, 992, DateTimeKind.Local).AddTicks(1994), "aleffmds@gmail.com", "Aleff", "pt-BR", null, "Moura da Silva", "aleffmoura", "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", null, false, new Guid("e36d8c0c-7e31-4b7f-9c25-cf5ab0516b6c"), null, null, new DateTime(2020, 6, 10, 19, 52, 21, 992, DateTimeKind.Local).AddTicks(2653) });

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
