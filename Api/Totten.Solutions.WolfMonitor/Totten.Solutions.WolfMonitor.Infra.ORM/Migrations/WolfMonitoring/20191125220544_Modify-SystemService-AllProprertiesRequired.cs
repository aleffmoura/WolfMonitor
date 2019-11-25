using Microsoft.EntityFrameworkCore.Migrations;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitoring
{
    public partial class ModifySystemServiceAllProprertiesRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "SystemServices",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "SystemServices",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);
        }
    }
}
