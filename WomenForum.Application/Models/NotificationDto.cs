using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Models;

public class NotificationDto : BaseDtoWithId
{
    public UserDto Receiver { get; set; } = null!;

    public UserDto? TriggeredBy { get; set; }

    public Guid? TargetId { get; set; }
    public NotificationSource? Source { get; set; }
    
    public NotificationType Type { get; set; }

    public DateTime? ReadAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}