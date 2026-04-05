using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class CommunityMember : BaseDbEntityWithId
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid CommunityId { get; set; }
    public Community Community { get; set; } = null!;

    public CommunityRole Role { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public bool IsBanned { get; set; }
}