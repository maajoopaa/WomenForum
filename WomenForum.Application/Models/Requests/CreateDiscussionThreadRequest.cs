namespace WomenForum.Models.Requests;

public class CreateDiscussionThreadRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}