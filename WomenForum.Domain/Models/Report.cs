using Templates.Models;
using WomenForum.Domain.Enums;

namespace WomenForum.Domain.Models;

public class Report : BaseDbEntityWithId
{
    public Guid ReportedById { get; set; }
    public User ReportedBy { get; set; } = null!;
    
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }
}