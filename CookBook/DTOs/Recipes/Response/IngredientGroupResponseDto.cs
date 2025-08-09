namespace CookBook.DTOs.Recipes.Response;

public class IngredientGroupResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<IngredientResponseDto> Ingredients { get; set; } = new();
}