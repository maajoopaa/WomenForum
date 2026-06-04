using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class User : BaseDbEntityWithId
{
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public DateTime BirthDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? DeletedAt { get; set; }
    
    public DateTime LastLogin { get; set; }
    
    public string? Avatar { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;
    
    public Role Role { get; set; }
    
    public VisibilityType Visibility { get; set; }

    public virtual UserSettings UserSettings { get; set; } = null!;
    
    public virtual List<Like> Likes { get; set; } = [];
    
    public virtual List<Comment> Comments { get; set; } = [];
    
    public virtual List<Message> Messages { get; set; } = [];

    public virtual List<Subscription> Following { get; set; } = [];
    
    public virtual List<Subscription> Followers { get; set; } = [];
    
    public virtual List<Community> Communities { get; set; } = [];
    
    public virtual List<CommunityMember> CommunityMemberships { get; set; } = [];
    
    public virtual List<Post> Posts { get; set; } = [];
    
    public virtual List<Warning> Warnings { get; set; } = [];
    
    public virtual List<Report> Reports { get; set; } = [];
}