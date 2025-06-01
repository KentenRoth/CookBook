namespace CookBook.Models;

public class IngredientGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RecipeId { get; set; }
    
    public Recipe Recipe { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}