namespace CookBook.DTOs.Recipes.Request;

public class CreateIngredientGroupDto
{
    public string Name { get; set; } = string.Empty;
    public List<CreateIngredientDto> Ingredients { get; set; } = new List<CreateIngredientDto>();
}