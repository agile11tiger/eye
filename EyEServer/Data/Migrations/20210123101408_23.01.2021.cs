using Microsoft.EntityFrameworkCore.Migrations;

namespace EyEServer.Migrations;

public partial class _23012021 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Country",
            table: "Links",
            type: "nvarchar(max)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Country",
            table: "Links");
    }
}
