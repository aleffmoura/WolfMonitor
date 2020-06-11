using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitor
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    FantasyName = table.Column<string>(nullable: false),
                    Cnpj = table.Column<string>(nullable: false),
                    StateRegistration = table.Column<string>(nullable: true),
                    MunicipalRegistration = table.Column<string>(nullable: true),
                    Cnae = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    ProfileIdentifier = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    AgentId = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    UserWhoCreatedId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    UserWhoCreatedId = table.Column<Guid>(nullable: false),
                    UserWhoCreatedName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: false),
                    MachineName = table.Column<string>(nullable: true),
                    LocalIp = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    HostAddress = table.Column<string>(nullable: true),
                    Login = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    Configured = table.Column<bool>(nullable: false),
                    ReadItemsMonitoringByArchive = table.Column<bool>(nullable: false),
                    FirstConnection = table.Column<DateTime>(nullable: true),
                    LastConnection = table.Column<DateTime>(nullable: true),
                    LastUpload = table.Column<DateTime>(nullable: true),
                    ProfileIdentifier = table.Column<Guid>(nullable: false),
                    ProfileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agents_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "Cnae", "Cnpj", "CreatedIn", "Email", "FantasyName", "MunicipalRegistration", "Name", "Phone", "Removed", "StateRegistration", "UpdatedIn" },
                values: new object[] { new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), "Rua Cicero Lourenço, Mossoró/RN", "", "35.344.681/0001-90", new DateTime(2020, 6, 10, 19, 52, 41, 495, DateTimeKind.Local).AddTicks(4607), "aleffmds@gmail.com", "tottemsolutions", "", "ALEFF MOURA DA SILVA", "(49) 9 9914-6350", false, "", new DateTime(2020, 6, 10, 19, 52, 41, 496, DateTimeKind.Local).AddTicks(3113) });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "CompanyId", "Configured", "CreatedIn", "DisplayName", "FirstConnection", "HostAddress", "HostName", "LastConnection", "LastUpload", "LocalIp", "Login", "MachineName", "Password", "ProfileIdentifier", "ProfileName", "ReadItemsMonitoringByArchive", "Removed", "UpdatedIn", "UserWhoCreatedId", "UserWhoCreatedName" },
                values: new object[] { new Guid("aa22550f-0365-464a-9dc5-1e971a91879d"), new Guid("c576cf93-370c-4464-21f9-08d763d27d75"), false, new DateTime(2020, 6, 10, 19, 52, 41, 498, DateTimeKind.Local).AddTicks(6544), "Servidor BR 1", null, null, null, null, null, null, "servidor1", null, "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=", new Guid("00000000-0000-0000-0000-000000000000"), null, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f75a1881-0fd6-4273-9d23-c59018788201"), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_CompanyId",
                table: "Agents",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_UserWhoCreatedId",
                table: "Agents",
                column: "UserWhoCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_AgentId",
                table: "Profiles",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_CompanyId",
                table: "Profiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserWhoCreatedId",
                table: "Profiles",
                column: "UserWhoCreatedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
