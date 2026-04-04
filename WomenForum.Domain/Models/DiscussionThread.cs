using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class DiscussionThread : BaseDbEntityWithId
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public VisibilityTypes Visibility { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? DeletedAt { get; set; }
    
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public List<Message> Messages { get; set; } = [];
}