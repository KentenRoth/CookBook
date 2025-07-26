using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class FavoriteRecipe
{
    [MaxLength(450)]
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;

    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
}