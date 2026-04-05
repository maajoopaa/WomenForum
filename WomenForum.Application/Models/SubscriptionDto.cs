using Templates.Models;

namespace WomenForum.Models;

public class SubscriptionDto : BaseDtoWithId
{
    public UserDto Subscriber { get; set; } = null!;
    
    public UserDto TargetUser { get; set; } = null!;
    
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}