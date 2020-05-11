using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitoring
{
    public partial class AddItemHistoric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    MonitoredAt = table.Column<DateTime>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Historic_ItemId",
                table: "Historic",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historic");
        }
    }
}
