using Templates.Models;

namespace WomenForum.Domain.Models;

public class Subscription : BaseDbEntityWithId
{
    public Guid SubscriberId { get; set; }
    public virtual User Subscriber { get; set; } = null!;
    
    public Guid TargetUserId { get; set; }
    public virtual User TargetUser { get; set; } = null!;
    
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}