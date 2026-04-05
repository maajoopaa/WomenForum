namespace WomenForum.Models.Requests;

public class UpdateDiscussionThreadRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}