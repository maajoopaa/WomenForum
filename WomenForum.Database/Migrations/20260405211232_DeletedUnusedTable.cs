using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomenForum.Database.Migrations
{
    /// <inheritdoc />
    public partial class DeletedUnusedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscussionThreadJoinRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscussionThreadJoinRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionThreadJoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscussionThreadJoinRequests_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscussionThreadJoinRequests_Users_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscussionThreadJoinRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionThreadJoinRequests_CommunityId",
                table: "DiscussionThreadJoinRequests",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionThreadJoinRequests_ReviewedById",
                table: "DiscussionThreadJoinRequests",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionThreadJoinRequests_UserId",
                table: "DiscussionThreadJoinRequests",
                column: "UserId");
        }
    }
}
