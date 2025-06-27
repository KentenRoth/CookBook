using System.ComponentModel.DataAnnotations;

namespace CookBook.DTOs.Account.Request;

public class LoginAccountRequestDto
{
    [Required]
    public string EmailOrUsername { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}