using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class Recipe
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    
    [MaxLength(450)]
    public required string UserId { get; set; }
    
    public ICollection<IngredientGroup> IngredientGroups { get; set; } = new List<IngredientGroup>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public required AppUser User { get; set; }
}