using System.Security.Claims;
using CookBook.DTOs.Recipes.Request;
using CookBook.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("api/recipe")]

public class RecipeController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    private readonly IRecipeService _recipeService;

    public RecipeController(IAccountService accountService, ITokenService tokenService, IRecipeService recipeService)
    {
        _accountService = accountService;
        _tokenService = tokenService;
        _recipeService = recipeService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRecipe(CreateRecipeDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var recipe = await _recipeService.CreateRecipe(dto, userId);

        return Ok(recipe);
    }

    [HttpGet("allrecipes")]
    public async Task<IActionResult> GetRecipes()
    {

        var recipes = await _recipeService.GetRecipes();

        return Ok(recipes);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var result = await _recipeService.DeleteRecipe(id, userId);

        if (!result.Success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("myrecipes")]
    public async Task<IActionResult> GetMyRecipes()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var recipes = await _recipeService.GetMyRecipes(userId);

        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipeById(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var recipe = await _recipeService.GetRecipeById(id, userId);

        if (!recipe.Success)
        {
            return NotFound();
        }

        return Ok(recipe);
    }
}