namespace CookBook.DTOs.Account.Request;

public class RegisterAccountRequestDto
{
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}