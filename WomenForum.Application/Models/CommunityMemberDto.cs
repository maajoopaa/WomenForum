using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Models;

public class CommunityMemberDto : BaseDtoWithId
{
    public UserDto User { get; set; } = null!;

    public CommunityRole Role { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public bool IsBanned { get; set; }
}