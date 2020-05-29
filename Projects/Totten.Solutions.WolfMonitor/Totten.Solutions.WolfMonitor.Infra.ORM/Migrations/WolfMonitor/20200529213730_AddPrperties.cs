using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitor
{
    public partial class AddPrperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: new Guid("54f96bbb-6621-4434-a196-f52e371ca020"));

            migrationBuilder.AddColumn<bool>(
                name: "ReadItemsMonitoringByArchive",
                table: "Agents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "CompanyId", "Configured", "CreatedIn", "DisplayName", "FirstConnection", "HostAddress", "HostName", "LastConnection", "LastUpload", "LocalIp", "Login", "MachineName", "Password", "ReadItemsMonitoringByArchive", "Removed", "UpdatedIn", "UserWhoCreatedId", "UserWhoCreatedName" },
                values: new object[] { new Guid("c9a526aa-9ab1-42c7-bce0-d3c818c99bd7"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), false, new DateTime(2020, 5, 29, 18, 37, 30, 2, DateTimeKind.Local).AddTicks(4201), "Servidor BR 1", null, null, null, null, null, null, "servidor1", null, "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f75a1881-0fd6-4273-9d23-c59018788201"), "Admin" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("c576cf93-370c-4464-21f9-08d763d27d75"),
                columns: new[] { "CreatedIn", "UpdatedIn" },
                values: new object[] { new DateTime(2020, 5, 29, 18, 37, 29, 999, DateTimeKind.Local).AddTicks(2546), new DateTime(2020, 5, 29, 18, 37, 30, 0, DateTimeKind.Local).AddTicks(1415) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: new Guid("c9a526aa-9ab1-42c7-bce0-d3c818c99bd7"));

            migrationBuilder.DropColumn(
                name: "ReadItemsMonitoringByArchive",
                table: "Agents");

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "CompanyId", "Configured", "CreatedIn", "DisplayName", "FirstConnection", "HostAddress", "HostName", "LastConnection", "LastUpload", "LocalIp", "Login", "MachineName", "Password", "Removed", "UpdatedIn", "UserWhoCreatedId", "UserWhoCreatedName" },
                values: new object[] { new Guid("54f96bbb-6621-4434-a196-f52e371ca020"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), false, new DateTime(2020, 5, 28, 19, 4, 13, 646, DateTimeKind.Local).AddTicks(3731), "Servidor BR 1", null, null, null, null, null, null, "servidor1", null, "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f75a1881-0fd6-4273-9d23-c59018788201"), "Admin" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("c576cf93-370c-4464-21f9-08d763d27d75"),
                columns: new[] { "CreatedIn", "UpdatedIn" },
                values: new object[] { new DateTime(2020, 5, 28, 19, 4, 13, 643, DateTimeKind.Local).AddTicks(2070), new DateTime(2020, 5, 28, 19, 4, 13, 644, DateTimeKind.Local).AddTicks(1000) });
        }
    }
}
