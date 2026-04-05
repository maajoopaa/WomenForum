namespace WomenForum.Models.Requests;

public class CreateMessageRequest
{
    public string Content { get; set; } = null!;
    
    public Guid? ParentMessageId { get; set; }
}