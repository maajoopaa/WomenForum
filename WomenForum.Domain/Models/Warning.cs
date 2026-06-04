using Templates.Models;

namespace WomenForum.Domain.Models;

public class Warning : BaseDbEntityWithId
{
    public string Message { get; set; } = null!;
    
    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
    
    public bool IsRead { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}