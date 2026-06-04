using Templates.Models;

namespace WomenForum.Models;

public class MessageDto : BaseDtoWithId
{
    public string Content { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public UserDto CreatedBy { get; set; } = null!;
    
    public MessageDto? ParentMessage { get; set; }
    
    public int ReplyCount { get; set; }
}