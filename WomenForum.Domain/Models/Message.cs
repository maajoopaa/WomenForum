using Templates.Models;

namespace WomenForum.Domain.Models;

public class Message : BaseDbEntityWithId
{
    public string Content { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Guid CreatedById { get; set; }
    public virtual User CreatedBy { get; set; } = null!;
    
    public Guid DiscussionThreadId { get; set; }
    public virtual DiscussionThread DiscussionThread { get; set; } = null!;
    
    public Guid? ParentMessageId { get; set; }
    public virtual Message? ParentMessage { get; set; }
    
    public virtual List<Message> Replies { get; set; } = [];
}