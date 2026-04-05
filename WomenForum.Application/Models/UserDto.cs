using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;

namespace WomenForum.Models;

public class UserDto : BaseDtoWithId
{
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public DateTime BirthDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? DeletedAt { get; set; }
    
    public DateTime LastLogin { get; set; }
    
    public string? Avatar { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public Role Role { get; set; }
    
    public VisibilityType Visibility { get; set; }

    public UserSettingsDto UserSettings { get; set; } = null!;
    
    public int FollowingCount { get; set; }
    
    public int FollowersCount { get; set; }
}