using Microsoft.EntityFrameworkCore.Migrations;

namespace EyEServer.Migrations;

public partial class _27012021 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "AniDbVotes",
            table: "Links",
            type: "int",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AniDbVotes",
            table: "Links");
    }
}
