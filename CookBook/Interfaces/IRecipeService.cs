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
    Task<ServiceResponseDto<List<RecipeResponseDto>>> GetMyRecipes(string userId);
    Task<ServiceResponseDto<List<RecipeResponseDto>>> GetRecipeByUser(string userId);
    Task<ServiceResponseDto<List<RecipeTagResponseDto>>> GetAllTags();
    Task<ServiceResponseDto<RecipeResponseDto>> UpdateRecipe(int recipeId, UpdateRecipeRequestDto dto, string userId);
    Task<ServiceResponseDto<List<RecipeResponseDto>>> SearchRecipes(string query, string? currentUserId = null);
}