using Templates.Models;

namespace WomenForum.Domain.Models;

public class Like : BaseDbEntityWithId
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Guid LikedById { get; set; }
    public virtual User LikedBy { get; set; } = null!;
    
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; } = null!;
}