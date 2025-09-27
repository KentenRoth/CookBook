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

    [HttpGet("user/{username}")]
    public async Task<IActionResult> GetRecipeByUser(string username)
    {
        var recipes = await _recipeService.GetRecipeByUser(username);

        if (!recipes.Success)
        {
            return NotFound();
        }

        return Ok(recipes);
    }

    [HttpGet("tags")]
    public async Task<IActionResult> GetAllTags()
    {
        var tags = await _recipeService.GetAllTags();

        return Ok(tags);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipe(int id, UpdateRecipeRequestDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var recipe = await _recipeService.UpdateRecipe(id, dto, userId);

        if (!recipe.Success)
        {
            return NotFound();
        }

        return Ok(recipe);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRecipes([FromQuery] string query)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var recipes = await _recipeService.SearchRecipes(query, userId);

        return Ok(recipes);
    }
    [HttpGet("filter")]
    public async Task<IActionResult> FilterRecipesByTags([FromQuery] string tags)
    {
        var filterTags = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(t => t.Trim())
                         .ToList();
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipes = await _recipeService.FilterRecipesByTags(filterTags, currentUserId);
        return Ok(recipes);
    }
}