namespace CookBook.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    public required string UserId { get; set; }
    
    public ICollection<IngredientGroup> IngredientGroups { get; set; } = new List<IngredientGroup>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public required AppUser User { get; set; }
}