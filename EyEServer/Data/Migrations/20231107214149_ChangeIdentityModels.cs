using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EyEServer.Migrations;

/// <inheritdoc />
public partial class ChangeIdentityModels : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Discriminator",
            table: "Links",
            type: "nvarchar(13)",
            maxLength: 13,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Discriminator",
            table: "Links",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(13)",
            oldMaxLength: 13);
    }
}
