namespace WomenForum.Models.Requests;

public class CreateCategoryRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public string? Logo { get; set; }
}