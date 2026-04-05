using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomenForum.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMember_Communities_CommunityId",
                table: "CommunityMember");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMember_Users_UserId",
                table: "CommunityMember");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Users_SubscriberId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Users_TargetUserId",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityMember",
                table: "CommunityMember");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "CommunityMember",
                newName: "CommunityMembers");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_TargetUserId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_TargetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_SubscriberId_TargetUserId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SubscriberId_TargetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMember_UserId_CommunityId",
                table: "CommunityMembers",
                newName: "IX_CommunityMembers_UserId_CommunityId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMember_CommunityId",
                table: "CommunityMembers",
                newName: "IX_CommunityMembers_CommunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityMembers",
                table: "CommunityMembers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CommunityJoinRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReviewedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityJoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityJoinRequests_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityJoinRequests_Users_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommunityJoinRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionThreadJoinRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReviewedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    TriggeredById = table.Column<Guid>(type: "uuid", nullable: true),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: true),
                    Source = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_TriggeredById",
                        column: x => x.TriggeredById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityJoinRequests_CommunityId",
                table: "CommunityJoinRequests",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityJoinRequests_ReviewedById",
                table: "CommunityJoinRequests",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityJoinRequests_UserId",
                table: "CommunityJoinRequests",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TriggeredById",
                table: "Notifications",
                column: "TriggeredById");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_Communities_CommunityId",
                table: "CommunityMembers",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_Users_UserId",
                table: "CommunityMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Users_SubscriberId",
                table: "Subscriptions",
                column: "SubscriberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Users_TargetUserId",
                table: "Subscriptions",
                column: "TargetUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_Communities_CommunityId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_Users_UserId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Users_SubscriberId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Users_TargetUserId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "CommunityJoinRequests");

            migrationBuilder.DropTable(
                name: "DiscussionThreadJoinRequests");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityMembers",
                table: "CommunityMembers");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "Subscription");

            migrationBuilder.RenameTable(
                name: "CommunityMembers",
                newName: "CommunityMember");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_TargetUserId",
                table: "Subscription",
                newName: "IX_Subscription_TargetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SubscriberId_TargetUserId",
                table: "Subscription",
                newName: "IX_Subscription_SubscriberId_TargetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMembers_UserId_CommunityId",
                table: "CommunityMember",
                newName: "IX_CommunityMember_UserId_CommunityId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMembers_CommunityId",
                table: "CommunityMember",
                newName: "IX_CommunityMember_CommunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityMember",
                table: "CommunityMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMember_Communities_CommunityId",
                table: "CommunityMember",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMember_Users_UserId",
                table: "CommunityMember",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Users_SubscriberId",
                table: "Subscription",
                column: "SubscriberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Users_TargetUserId",
                table: "Subscription",
                column: "TargetUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
