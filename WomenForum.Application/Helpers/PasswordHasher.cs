namespace WomenForum.Helpers;

public class PasswordHasher
{
    public static string HashPassword(string password) => 
        BCrypt.Net.BCrypt.HashPassword(password);

    public static bool VerifyPassword(string hashedPassword, string password) => 
        BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}