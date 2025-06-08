using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class Ingredient
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Amount { get; set; } = string.Empty;
    public int IngredientGroupId { get; set; }
    
    public IngredientGroup IngredientGroup { get; set; } = null!;
}