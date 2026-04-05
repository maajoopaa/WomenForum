using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class Report : BaseDbEntityWithId
{
    public Guid ReportedById { get; set; }
    public User ReportedBy { get; set; } = null!;

    public Guid? PostId { get; set; }
    public Post? Post { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public Guid? CommunityId { get; set; }
    public Community? Community { get; set; }

    public Guid? ThreadId { get; set; }
    public DiscussionThread? Thread { get; set; }

    public string Description { get; set; } = null!;
    
    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ResolvedAt { get; set; }
}