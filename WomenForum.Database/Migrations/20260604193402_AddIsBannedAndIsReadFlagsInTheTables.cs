using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomenForum.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBannedAndIsReadFlagsInTheTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Warnings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Users");
        }
    }
}
