using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class UserActivity : BaseDbEntityWithId
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public ActivityType Type { get; set; }
    
    public Guid? TargetId { get; set; }
    
    public string Description { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}