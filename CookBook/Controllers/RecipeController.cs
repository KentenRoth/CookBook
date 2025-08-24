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
}