namespace CookBook.DTOs.Account.Request;

public class UpdateUserRequestDto
{
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? NewPassword { get; set; }
    public required string CurrentPassword { get; set; }
}