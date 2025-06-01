namespace CookBook.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public int IngredientGroupId { get; set; }
    
    public IngredientGroup IngredientGroup { get; set; } = new IngredientGroup();
}