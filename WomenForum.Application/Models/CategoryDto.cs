using Templates.Models;

namespace WomenForum.Models;

public class CategoryDto : BaseDtoWithId
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public string? Logo { get; set; }
}