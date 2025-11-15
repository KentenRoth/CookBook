namespace CookBook.DTOs.Account.Response;

public class UserSettingsResponseDto
{
    public required string ColorMode { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;
}