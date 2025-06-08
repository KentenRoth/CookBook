using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class RefreshTokens
{
    public int Id { get; set; }
    
    [MaxLength(512)]
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    [MaxLength(45)]
    public string CreatedByIp { get; set; } = string.Empty;
    
    [MaxLength(45)]
    public string? RevokedByIp { get; set; }
    public DateTime? RevokedAt { get; set; }
    public bool IsRevoked { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; }
    
}