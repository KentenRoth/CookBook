namespace CookBook.DTOs.Recipes.Request;

public class CreateRecipeDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Servings { get; set; }
    public bool IsPublic { get; set; }
    
    public int? PrepTimeMinutes { get; set; }
    public int? CookTimeMinutes { get; set; }
    
    public List<CreateRecipeStepDto> Steps { get; set; } = new List<CreateRecipeStepDto>();
    public List<CreateIngredientGroupDto> IngredientGroups { get; set; } = new List<CreateIngredientGroupDto>();
    public List<string> Tags { get; set; } = new List<string>();
}