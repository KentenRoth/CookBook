namespace CookBook.DTOs.Account.Request;

public class RegisterAccountRequestDto
{
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}