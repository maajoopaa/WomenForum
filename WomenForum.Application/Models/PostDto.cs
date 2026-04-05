using Templates.Models;

namespace WomenForum.Models;

public class PostDto : BaseDtoWithId
{
    public string Title { get; set; } = null!;

    public string HtmlContent { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? DeletedAt { get; set; }
    
    public UserDto AuthorUser { get; set; } = null!;

    public List<LikeDto> Likes { get; set; } = [];

    public List<CommentDto> Comments { get; set; } = [];
}