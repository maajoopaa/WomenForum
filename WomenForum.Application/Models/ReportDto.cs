using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Models;

public class ReportDto : BaseDtoWithId
{
    public UserDto ReportedBy { get; set; } = null!;

    public PostDto? Post { get; set; }

    public UserDto? User { get; set; }

    public CommunityDto? Community { get; set; }

    public DiscussionThreadDto? Thread { get; set; }

    public string Description { get; set; } = null!;
    
    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ResolvedAt { get; set; }
}