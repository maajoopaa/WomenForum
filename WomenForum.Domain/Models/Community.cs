using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class Community : BaseDbEntityWithId
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    public string? Avatar { get; set; }
    
    public DateTime? DeletedAt { get; set; }

    public VisibilityType Visibility { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    
    public List<Post> Posts { get; set; } = [];
    
    public List<CommunityMember> Members { get; set; } = [];
}