namespace CookBook.DTOs.Account.Response;

public class MeAccountResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public object UserSettings { get; set; } = new object();
    
}