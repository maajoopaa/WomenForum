namespace WomenForum.Exceptions;

public class BadRequestException(string message) : Exception(message)
{
    public int StatusCode { get; set; } = 400;
}