using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class UserSettings
{
    public int Id { get; set; }
    
    [MaxLength(450)]
    public string UserId { get; set; } = string.Empty;
    
    [MaxLength(10)]
    public required string ColorMode { get; set; } = "light";
    
    public required AppUser User { get; set; }
}