namespace CookBook.DTOs.Account.Response;

public class MeAccountResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public object UserSettings { get; set; } = new object();
    
}