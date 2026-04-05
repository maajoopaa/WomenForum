using WomenForum.Domain.Enums;

namespace WomenForum.Models.Requests;

public class CreateCommunityRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public string? Avatar { get; set; }
    
    public VisibilityType Visibility { get; set; }

    public Guid CategoryId { get; set; }
}