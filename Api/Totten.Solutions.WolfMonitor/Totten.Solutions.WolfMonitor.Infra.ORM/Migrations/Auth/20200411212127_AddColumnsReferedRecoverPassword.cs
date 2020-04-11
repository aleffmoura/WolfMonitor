using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.Auth
{
    public partial class AddColumnsReferedRecoverPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("527d5006-c3bf-4d97-8c7d-387a967894fe"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6a5ecc23-e4ea-4077-ac04-6836e188ab0b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ccc8b552-9b77-4318-88ba-56beeea131fe"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("93409973-4e84-44f8-8997-b452921dba34"));

            migrationBuilder.AddColumn<string>(
                name: "RecoverSolicitationCode",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenSolicitationCode",
                table: "Users",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f91a2366-c469-412a-9197-976a90516272"),
                columns: new[] { "CreatedIn", "UpdatedIn" },
                values: new object[] { new DateTime(2020, 4, 11, 18, 21, 26, 698, DateTimeKind.Local).AddTicks(3043), new DateTime(2020, 4, 11, 18, 21, 26, 698, DateTimeKind.Local).AddTicks(3044) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedIn", "Level", "Name", "Removed", "UpdatedIn" },
                values: new object[,]
                {
                    { new Guid("c2b5f932-2486-40c2-acd4-d4dfaee23c0a"), new DateTime(2020, 4, 11, 18, 21, 26, 695, DateTimeKind.Local).AddTicks(7639), 0, "Agent", false, new DateTime(2020, 4, 11, 18, 21, 26, 696, DateTimeKind.Local).AddTicks(4672) },
                    { new Guid("a0ecc468-516a-46a7-8134-b65857c1edb8"), new DateTime(2020, 4, 11, 18, 21, 26, 698, DateTimeKind.Local).AddTicks(2818), 1, "User", false, new DateTime(2020, 4, 11, 18, 21, 26, 698, DateTimeKind.Local).AddTicks(2832) },
                    { new Guid("259c8774-2813-4f73-9b63-fc131afbbbc4"), new DateTime(2020, 4, 11, 18, 21, 26, 698, DateTimeKind.Local).AddTicks(3088), 3, "System", false, new DateTime(2020, 4, 11, 18, 21, 26, 698, DateTimeKind.Local).AddTicks(3089) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastName", "Login", "Password", "RecoverSolicitationCode", "Removed", "RoleId", "Token", "TokenSolicitationCode", "UpdatedIn" },
                values: new object[] { new Guid("5c37de9d-7eb2-4d2b-a4ae-a5248bd6bfce"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "10685805425", new DateTime(2020, 4, 11, 18, 21, 26, 724, DateTimeKind.Local).AddTicks(5352), "aleffmds@gmail.com", "Aleff", "pt-BR", "Moura da Silva", "aleffmoura", "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", null, false, new Guid("259c8774-2813-4f73-9b63-fc131afbbbc4"), null, null, new DateTime(2020, 4, 11, 18, 21, 26, 724, DateTimeKind.Local).AddTicks(5369) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a0ecc468-516a-46a7-8134-b65857c1edb8"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c2b5f932-2486-40c2-acd4-d4dfaee23c0a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5c37de9d-7eb2-4d2b-a4ae-a5248bd6bfce"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("259c8774-2813-4f73-9b63-fc131afbbbc4"));

            migrationBuilder.DropColumn(
                name: "RecoverSolicitationCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenSolicitationCode",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f91a2366-c469-412a-9197-976a90516272"),
                columns: new[] { "CreatedIn", "UpdatedIn" },
                values: new object[] { new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8447), new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8449) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedIn", "Level", "Name", "Removed", "UpdatedIn" },
                values: new object[,]
                {
                    { new Guid("6a5ecc23-e4ea-4077-ac04-6836e188ab0b"), new DateTime(2020, 2, 2, 12, 54, 2, 704, DateTimeKind.Local).AddTicks(4752), 0, "Agent", false, new DateTime(2020, 2, 2, 12, 54, 2, 705, DateTimeKind.Local).AddTicks(4502) },
                    { new Guid("527d5006-c3bf-4d97-8c7d-387a967894fe"), new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8287), 1, "User", false, new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8306) },
                    { new Guid("93409973-4e84-44f8-8997-b452921dba34"), new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8539), 3, "System", false, new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8541) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Cpf", "CreatedIn", "Email", "FirstName", "Language", "LastName", "Login", "Password", "Removed", "RoleId", "UpdatedIn" },
                values: new object[] { new Guid("ccc8b552-9b77-4318-88ba-56beeea131fe"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "10685805425", new DateTime(2020, 2, 2, 12, 54, 2, 723, DateTimeKind.Local).AddTicks(193), "aleffmds@gmail.com", "Aleff", "pt-BR", "Moura da Silva", "aleffmoura", "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", false, new Guid("93409973-4e84-44f8-8997-b452921dba34"), new DateTime(2020, 2, 2, 12, 54, 2, 723, DateTimeKind.Local).AddTicks(230) });
        }
    }
}
