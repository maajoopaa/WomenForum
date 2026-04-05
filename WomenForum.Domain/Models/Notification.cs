using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class Notification : BaseDbEntityWithId
{
    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;

    public Guid? TriggeredById { get; set; }
    public User? TriggeredBy { get; set; }

    public Guid? TargetId { get; set; }
    
    public NotificationSource? Source { get; set; }
    
    public NotificationType Type { get; set; }

    public DateTime? ReadAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}