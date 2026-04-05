using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;

namespace WomenForum.Models;

public class CommunityJoinRequestDto : BaseDtoWithId
{
    public UserDto User { get; set; } = null!;

    public JoinRequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    public UserDto ReviewedBy { get; set; } = null!;

    public string? Message { get; set; }
}