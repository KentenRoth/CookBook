using System.ComponentModel.DataAnnotations;

namespace CookBook.DTOs.Account.Response;

public class UpdateUserSettingsRequestDto
{
    [Required]
    public string ColorMode { get; set; }
}