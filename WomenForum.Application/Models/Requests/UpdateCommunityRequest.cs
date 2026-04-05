namespace WomenForum.Models.Requests;

public class UpdateCommunityRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public string? Avatar { get; set; }
    
    public Guid CategoryId { get; set; }
}