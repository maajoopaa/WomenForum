using Templates.Models;

namespace WomenForum.Models;

public class DiscussionThreadDto : BaseDtoWithId
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? DeletedAt { get; set; }
    
    public UserDto CreatedBy { get; set; } = null!;

    public List<MessageDto> Messages { get; set; } = [];
}