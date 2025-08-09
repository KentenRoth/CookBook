using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class IngredientGroup
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public int RecipeId { get; set; }

    public Recipe Recipe { get; set; } = null!;
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}