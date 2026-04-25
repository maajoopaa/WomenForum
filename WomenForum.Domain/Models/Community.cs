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
    public virtual Category Category { get; set; } = null!;
    
    public Guid CreatedById { get; set; }
    public virtual User CreatedBy { get; set; } = null!;
    
    public virtual List<Post> Posts { get; set; } = [];
    
    public virtual List<CommunityMember> Members { get; set; } = [];
}