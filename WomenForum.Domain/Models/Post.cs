using Templates.Models;

namespace WomenForum.Domain.Models;

public class Post : BaseDbEntityWithId
{
    public string Title { get; set; } = null!;

    public string HtmlContent { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? DeletedAt { get; set; }
    
    public Guid AuthorUserId { get; set; }
    public virtual User AuthorUser { get; set; } = null!;

    public Guid? CommunityId { get; set; }
    public virtual Community? Community { get; set; }

    public virtual List<Like> Likes { get; set; } = [];

    public virtual List<Comment> Comments { get; set; } = [];

    public virtual List<Image> PinnedImages { get; set; } = [];
}