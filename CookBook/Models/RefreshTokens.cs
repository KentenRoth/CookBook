namespace CookBook.Models;

public class RefreshTokens
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; }
    
}