namespace CookBook.DTOs.Account.Response;

public class UpdateUserResponseDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
}