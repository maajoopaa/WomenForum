using Templates.Models;

namespace WomenForum.Domain.Models;

public class Like : BaseDbEntityWithId
{
    public DateTime CreatedAt { get; set; }
    
    public Guid LikedById { get; set; }
    public User LikedBy { get; set; } = null!;
    
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}