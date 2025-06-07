namespace CookBook.Models;

public class UserSettings
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public required string ColorMode { get; set; } = "light";
    
    public required AppUser User { get; set; }
}