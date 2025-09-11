using CookBook.DTOs;
using CookBook.DTOs.Recipes.Request;
using CookBook.DTOs.Recipes.Response;

namespace CookBook.Interfaces;

public interface IRecipeService
{
    Task<ServiceResponseDto<RecipeResponseDto>> CreateRecipe(CreateRecipeDto dto, string userId);
    Task<ServiceResponseDto<EmptyDto>> DeleteRecipe(int recipeId, string userId);
    Task<ServiceResponseDto<List<RecipeResponseDto>>> GetRecipes();
    Task<ServiceResponseDto<RecipeResponseDto>> GetRecipeById(int id, string? userId = null);
}