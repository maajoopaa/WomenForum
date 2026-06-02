using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class CommunityJoinRequest : BaseDbEntityWithId
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public Guid CommunityId { get; set; }
    public virtual Community Community { get; set; } = null!;

    public JoinRequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    public Guid? ReviewedById { get; set; }
    public virtual User? ReviewedBy { get; set; }

    public string? Message { get; set; }
}