using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class UserSettings : BaseDbEntityWithId
{
    public Theme Theme { get; set; }
    
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}