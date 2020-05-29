using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitoring
{
    public partial class ChangePropertie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Interval",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "AboutCurrentValue",
                table: "Items",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutCurrentValue",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Default",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Interval",
                table: "Items",
                nullable: false,
                defaultValue: 0);
        }
    }
}
