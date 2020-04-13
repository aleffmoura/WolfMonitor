using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitoring
{
    public partial class AddTableSolicitationHistoric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MonitoredAt",
                table: "Historic",
                nullable: true,
                oldClrType: typeof(DateTime));

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
                name: "SolicitationsHistoric");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MonitoredAt",
                table: "Historic",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
