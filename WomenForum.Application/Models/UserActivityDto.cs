using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Models;

public class UserActivityDto : BaseDtoWithId
{
    public Guid UserId { get; set; }
    
    public ActivityType Type { get; set; }
    
    public Guid? TargetId { get; set; }
    
    public string Description { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
}
