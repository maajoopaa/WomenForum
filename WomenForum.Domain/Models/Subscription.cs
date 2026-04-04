using Templates.Models;

namespace WomenForum.Domain.Models;

public class Subscription : BaseDbEntityWithId
{
    public Guid SubscriberId { get; set; }

    public User Subscriber { get; set; } = null!;
    
    public Guid? TargetUserId { get; set; }
    public User? TargetUser { get; set; }
    
    public Guid? TargetCommunityId { get; set; }
    public Community? TargetCommunity { get; set; }
    
    public DateTime SubscribedAt { get; set; }
}