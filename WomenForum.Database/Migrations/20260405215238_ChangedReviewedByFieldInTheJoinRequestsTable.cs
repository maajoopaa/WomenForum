using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomenForum.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangedReviewedByFieldInTheJoinRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityJoinRequests_Users_ReviewedById",
                table: "CommunityJoinRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewedById",
                table: "CommunityJoinRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityJoinRequests_Users_ReviewedById",
                table: "CommunityJoinRequests",
                column: "ReviewedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityJoinRequests_Users_ReviewedById",
                table: "CommunityJoinRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewedById",
                table: "CommunityJoinRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityJoinRequests_Users_ReviewedById",
                table: "CommunityJoinRequests",
                column: "ReviewedById",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
