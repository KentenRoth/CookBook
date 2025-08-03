namespace CookBook.DTOs.Recipes.Request;

public class CreateRecipeStepDto
{
    public int StepNumber { get; set; }
    public string Instruction { get; set; } = string.Empty;
}