namespace WomenForum.Models.Requests;

public class UpdatePostRequest
{
    public string Title { get; set; } = null!;

    public string HtmlContent { get; set; } = null!;
}