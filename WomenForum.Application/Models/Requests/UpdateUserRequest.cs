namespace WomenForum.Models.Requests;

public class UpdateUserRequest
{
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public DateTime BirthDate { get; set; }
    
    public string? Avatar { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
}