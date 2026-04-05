using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;

namespace WomenForum.Models;

public class CommunityDto : BaseDtoWithId
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    public string? Avatar { get; set; }
    
    public DateTime? DeletedAt { get; set; }

    public VisibilityType Visibility { get; set; }

    public CategoryDto Category { get; set; } = null!;
    
    public UserDto CreatedBy { get; set; } = null!;
    
    public List<PostDto> Posts { get; set; } = [];
    
    public List<CommunityMemberDto> Members { get; set; } = [];
}