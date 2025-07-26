using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CookBook.Models;

public class AppUser : IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public ICollection<RefreshTokens> RefreshTokens { get; set; } = new List<RefreshTokens>();
    public UserSettings? UserSettings { get; set; }
    public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();

}