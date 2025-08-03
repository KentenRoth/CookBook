namespace CookBook.DTOs.Recipes.Request;

public class CreateIngredientDto
{
    public string Name { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
}