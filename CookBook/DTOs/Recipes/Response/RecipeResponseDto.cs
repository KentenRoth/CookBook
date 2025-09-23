namespace CookBook.DTOs.Recipes.Response;

public class RecipeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Servings { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }

    public int? PrepTimeMinutes { get; set; }
    public int? CookTimeMinutes { get; set; }
    public string About { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string CreatorName { get; set; } = string.Empty;

    public List<RecipeStepResponseDto> Steps { get; set; } = new();
    public List<IngredientGroupResponseDto> IngredientGroups { get; set; } = new();
    public List<string> Tags { get; set; } = new List<string>();

}