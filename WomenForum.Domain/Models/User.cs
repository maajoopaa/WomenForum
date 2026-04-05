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

    public UserSettings UserSettings { get; set; } = null!;
    
    public List<Like> Likes { get; set; } = [];
    
    public List<Comment> Comments { get; set; } = [];
    
    public List<Message> Messages { get; set; } = [];

    public List<Subscription> Following { get; set; } = [];
    
    public List<Subscription> Followers { get; set; } = [];
    
    public List<Community> Communities { get; set; } = [];
    
    public List<CommunityMember> CommunityMemberships { get; set; } = [];
    
    public List<Post> Posts { get; set; } = [];
    
    public List<Report> Reports { get; set; } = [];
}