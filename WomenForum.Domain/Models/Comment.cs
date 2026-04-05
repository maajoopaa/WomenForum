using Templates.Models;

namespace WomenForum.Domain.Models;

public class Comment : BaseDbEntityWithId
{
    public string Content { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}