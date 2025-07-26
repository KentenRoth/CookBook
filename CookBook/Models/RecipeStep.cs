namespace CookBook.Models;

public class RecipeStep
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public required Recipe Recipe { get; set; }

    public int StepNumber { get; set; } 

    public string Instruction { get; set; } = string.Empty;
}