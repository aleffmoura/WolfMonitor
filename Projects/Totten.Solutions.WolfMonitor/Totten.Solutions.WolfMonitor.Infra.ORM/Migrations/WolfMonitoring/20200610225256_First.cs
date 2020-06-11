using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitoring
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 250, nullable: false),
                    AboutCurrentValue = table.Column<string>(nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false),
                    LastValue = table.Column<string>(nullable: true),
                    Default = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UserIdWhoAdd = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    AgentId = table.Column<Guid>(nullable: false),
                    MonitoredAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historic",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false),
                    MonitoredAt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historic_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitationsHistoric",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    CreatedIn = table.Column<DateTime>(nullable: false),
                    UpdatedIn = table.Column<DateTime>(nullable: false),
                    AgentId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    SolicitationType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 250, nullable: false),
                    NewValue = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitationsHistoric", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitationsHistoric_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Historic_ItemId",
                table: "Historic",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_AgentId",
                table: "Items",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CompanyId",
                table: "Items",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserIdWhoAdd",
                table: "Items",
                column: "UserIdWhoAdd");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitationsHistoric_AgentId",
                table: "SolicitationsHistoric",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitationsHistoric_CompanyId",
                table: "SolicitationsHistoric",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitationsHistoric_ItemId",
                table: "SolicitationsHistoric",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitationsHistoric_UserId",
                table: "SolicitationsHistoric",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historic");

            migrationBuilder.DropTable(
                name: "SolicitationsHistoric");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
