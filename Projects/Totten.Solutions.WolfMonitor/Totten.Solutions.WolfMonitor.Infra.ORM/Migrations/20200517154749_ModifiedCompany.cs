using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations
{
    public partial class ModifiedCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: new Guid("56159a2a-bbfd-413a-be9c-445bd8bbe894"));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Companies",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cnae",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Companies",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MunicipalRegistration",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Companies",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateRegistration",
                table: "Companies",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "CompanyId", "Configured", "CreatedIn", "DisplayName", "FirstConnection", "HostAddress", "HostName", "LastConnection", "LastUpload", "LocalIp", "Login", "MachineName", "Password", "Removed", "UpdatedIn", "UserWhoCreatedId", "UserWhoCreatedName" },
                values: new object[] { new Guid("ff77050f-0f93-41ff-8136-5b8c195a3aa5"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), false, new DateTime(2020, 5, 17, 12, 47, 48, 533, DateTimeKind.Local).AddTicks(6647), "Servidor BR 1", null, null, null, null, null, null, "servidor1", null, "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f91a2366-c469-412a-9197-976a90516272"), "Admin" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("c576cf93-370c-4464-21f9-08d763d27d75"),
                columns: new[] { "Address", "Cnae", "CreatedIn", "Email", "MunicipalRegistration", "Phone", "StateRegistration", "UpdatedIn" },
                values: new object[] { "Rua Cicero Lourenço, Mossoró/RN", "", new DateTime(2020, 5, 17, 12, 47, 48, 530, DateTimeKind.Local).AddTicks(5145), "aleffmds@gmail.com", "", "(49) 9 9914-6350", "", new DateTime(2020, 5, 17, 12, 47, 48, 531, DateTimeKind.Local).AddTicks(2180) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: new Guid("ff77050f-0f93-41ff-8136-5b8c195a3aa5"));

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Cnae",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "MunicipalRegistration",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "StateRegistration",
                table: "Companies");

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "CompanyId", "Configured", "CreatedIn", "DisplayName", "FirstConnection", "HostAddress", "HostName", "LastConnection", "LastUpload", "LocalIp", "Login", "MachineName", "Password", "Removed", "UpdatedIn", "UserWhoCreatedId", "UserWhoCreatedName" },
                values: new object[] { new Guid("56159a2a-bbfd-413a-be9c-445bd8bbe894"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), false, new DateTime(2020, 2, 2, 12, 53, 7, 848, DateTimeKind.Local).AddTicks(5113), "Servidor BR 1", null, null, null, null, null, null, "servidor1", null, "123456", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f91a2366-c469-412a-9197-976a90516272"), "Admin" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("c576cf93-370c-4464-21f9-08d763d27d75"),
                columns: new[] { "CreatedIn", "UpdatedIn" },
                values: new object[] { new DateTime(2020, 2, 2, 12, 53, 7, 844, DateTimeKind.Local).AddTicks(9898), new DateTime(2020, 2, 2, 12, 53, 7, 846, DateTimeKind.Local).AddTicks(3793) });
        }
    }
}
