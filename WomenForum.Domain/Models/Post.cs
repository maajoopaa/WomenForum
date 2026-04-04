using Templates.Models;

namespace WomenForum.Domain.Models;

public class Post : BaseDbEntityWithId
{
    public string Title { get; set; } = null!;

    public string HtmlContent { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ChangedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public Guid AuthorUserId { get; set; }
    public User AuthorUser { get; set; } = null!;

    public Guid? CommunityId { get; set; }
    public Community? Community { get; set; }

    public List<Like> Likes { get; set; } = [];

    public List<Comment> Comments { get; set; } = [];
    
    public List<Report> Reports { get; set; } = [];
}