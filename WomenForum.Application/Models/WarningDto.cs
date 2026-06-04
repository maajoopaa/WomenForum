using Templates.Models;

namespace WomenForum.Models;

public class WarningDto : BaseDtoWithId
{
    public string Message { get; set; } = null!;
    
    public UserDto User { get; set; } = null!;
    
    public bool IsRead { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}