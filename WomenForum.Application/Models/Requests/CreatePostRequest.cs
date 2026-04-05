namespace WomenForum.Models.Requests;

public class CreatePostRequest
{
    public string Title { get; set; } = null!;

    public string HtmlContent { get; set; } = null!;
    
    public Guid? CommunityId { get; set; }
}