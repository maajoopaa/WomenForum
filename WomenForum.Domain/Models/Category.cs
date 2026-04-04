using Templates.Models;

namespace WomenForum.Domain.Models;

public class Category : BaseDbEntityWithId
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public string? Logo { get; set; }
}