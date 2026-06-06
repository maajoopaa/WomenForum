using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class Notification : BaseDbEntityWithId
{
    public Guid ReceiverId { get; set; }
    public virtual User Receiver { get; set; }

    public Guid? TriggeredById { get; set; }
    public virtual User? TriggeredBy { get; set; }

    public Guid? TargetId { get; set; }
    
    public string? Message { get; set; }
    
    public NotificationSource? Source { get; set; }
    
    public NotificationType Type { get; set; }

    public DateTime? ReadAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}