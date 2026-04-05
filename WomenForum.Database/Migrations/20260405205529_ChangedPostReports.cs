using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomenForum.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPostReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_PostId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PostId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Reports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PostId1",
                table: "Reports",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId1",
                table: "Reports",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_PostId1",
                table: "Reports",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
