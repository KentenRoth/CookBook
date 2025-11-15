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
    public string About { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    public string RecipeImageUrl { get; set; } = string.Empty;


    [MaxLength(450)]
    public string UserId { get; set; }

    public ICollection<IngredientGroup> IngredientGroups { get; set; } = new List<IngredientGroup>();
    public ICollection<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public AppUser User { get; set; }
    public ICollection<FavoriteRecipe> FavoritedBy { get; set; } = new List<FavoriteRecipe>();
    public ICollection<RecipeImage> Images { get; set; } = new List<RecipeImage>();
    

}