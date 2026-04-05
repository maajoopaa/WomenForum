using Templates.Models;

namespace WomenForum.Models;

public class LikeDto : BaseDtoWithId
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public UserDto LikedBy { get; set; } = null!;
}