using Microsoft.EntityFrameworkCore;
using WomenForum.Domain.Models;

namespace WomenForum.Database;

public class WomenForumDbContext(DbContextOptions<WomenForumDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Community> Communities { get; set; }
    public DbSet<DiscussionThread> DiscussionThreads { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Subscriber)
            .WithMany(u => u.Following)
            .HasForeignKey(s => s.SubscriberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.TargetUser)
            .WithMany(u => u.Followers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.TargetCommunity)
            .WithMany(c => c.Followers)
            .HasForeignKey(s => s.TargetCommunityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Message>()
            .HasOne(m => m.ParentMessage)
            .WithMany(m => m.Replies)
            .HasForeignKey(m => m.ParentMessageId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Like>()
            .HasIndex(l => new { l.LikedById, l.PostId })
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<Subscription>()
            .HasCheckConstraint(
                "CK_Subscription_Target",
                    @"(
                (""TargetUserId"" IS NOT NULL AND ""TargetCommunityId"" IS NULL)
                OR
                (""TargetUserId"" IS NULL AND ""TargetCommunityId"" IS NOT NULL)
            )");
    }
}