namespace WomenForum.Models.Requests;

public class CreateWarningRequest
{
    public string Message { get; set; } = null!;
    
    public Guid UserId { get; set; }
}