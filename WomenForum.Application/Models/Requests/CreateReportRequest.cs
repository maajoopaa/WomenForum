namespace WomenForum.Models.Requests;

public class CreateReportRequest
{
    public Guid? PostId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? CommunityId { get; set; }

    public Guid? ThreadId { get; set; }

    public string Description { get; set; } = null!;
}