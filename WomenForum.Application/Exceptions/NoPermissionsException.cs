namespace WomenForum.Exceptions;

public class NoPermissionException(string message) : Exception(message)
{
    public int StatusCode { get; set; } = 403;
}