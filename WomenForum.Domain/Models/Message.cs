using Templates.Models;

namespace WomenForum.Domain.Models;

public class Message : BaseDbEntityWithId
{
    public string Content { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    
    public Guid DiscussionThreadId { get; set; }
    public DiscussionThread DiscussionThread { get; set; } = null!;
    
    public Guid? ParentMessageId { get; set; }
    public Message? ParentMessage { get; set; }
    
    public List<Message> Replies { get; set; } = [];
}