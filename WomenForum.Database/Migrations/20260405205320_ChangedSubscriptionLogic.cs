using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomenForum.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSubscriptionLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Categories_CategoryId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_CreatedById",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionThreads_Users_CreatedById",
                table: "DiscussionThreads");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_CreatedById",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_AuthorUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReportedById",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Communities_TargetCommunityId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_SubscriberId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_TargetCommunityId",
                table: "Subscription");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Subscription_Target",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "TargetCommunityId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "DiscussionThreads");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetUserId",
                table: "Subscription",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "Reports",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CommunityId",
                table: "Reports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId1",
                table: "Reports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ThreadId",
                table: "Reports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Reports",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommunityMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityMember_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMember_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SubscriberId_TargetUserId",
                table: "Subscription",
                columns: new[] { "SubscriberId", "TargetUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CommunityId",
                table: "Reports",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId1",
                table: "Reports",
                column: "PostId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ThreadId",
                table: "Reports",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMember_CommunityId",
                table: "CommunityMember",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMember_UserId_CommunityId",
                table: "CommunityMember",
                columns: new[] { "UserId", "CommunityId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Categories_CategoryId",
                table: "Communities",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_CreatedById",
                table: "Communities",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionThreads_Users_CreatedById",
                table: "DiscussionThreads",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_CreatedById",
                table: "Messages",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_AuthorUserId",
                table: "Posts",
                column: "AuthorUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Communities_CommunityId",
                table: "Reports",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_DiscussionThreads_ThreadId",
                table: "Reports",
                column: "ThreadId",
                principalTable: "DiscussionThreads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_PostId1",
                table: "Reports",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReportedById",
                table: "Reports",
                column: "ReportedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Categories_CategoryId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_CreatedById",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionThreads_Users_CreatedById",
                table: "DiscussionThreads");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_CreatedById",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_AuthorUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Communities_CommunityId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_DiscussionThreads_ThreadId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_PostId1",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReportedById",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "CommunityMember");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_SubscriberId_TargetUserId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CommunityId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PostId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ThreadId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_UserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reports");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetUserId",
                table: "Subscription",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "TargetCommunityId",
                table: "Subscription",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "Reports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "DiscussionThreads",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SubscriberId",
                table: "Subscription",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_TargetCommunityId",
                table: "Subscription",
                column: "TargetCommunityId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Subscription_Target",
                table: "Subscription",
                sql: "(\r\n                (\"TargetUserId\" IS NOT NULL AND \"TargetCommunityId\" IS NULL)\r\n                OR\r\n                (\"TargetUserId\" IS NULL AND \"TargetCommunityId\" IS NOT NULL)\r\n            )");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Categories_CategoryId",
                table: "Communities",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_CreatedById",
                table: "Communities",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionThreads_Users_CreatedById",
                table: "DiscussionThreads",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_CreatedById",
                table: "Messages",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_AuthorUserId",
                table: "Posts",
                column: "AuthorUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReportedById",
                table: "Reports",
                column: "ReportedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Communities_TargetCommunityId",
                table: "Subscription",
                column: "TargetCommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
