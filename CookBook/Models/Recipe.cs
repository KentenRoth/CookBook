using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Models;

public class Recipe
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public int? PrepTimeMinutes { get; set; }
    public int? CookTimeMinutes { get; set; }
    [Precision(5, 2)]
    public decimal Servings { get; set; }
    public bool IsPublic { get; set; } = true;
    
    [MaxLength(450)]
    public required string UserId { get; set; }
    
    public ICollection<IngredientGroup> IngredientGroups { get; set; } = new List<IngredientGroup>();
    public ICollection<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public required AppUser User { get; set; }
    public ICollection<FavoriteRecipe> FavoritedBy { get; set; } = new List<FavoriteRecipe>();

}