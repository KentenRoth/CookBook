namespace CookBook.DTOs.Account.Response;

public class MeAccountResponseDto
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required object UserSettings { get; set; } = new object();

}