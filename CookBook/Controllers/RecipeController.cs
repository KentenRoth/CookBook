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
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized("No Refresh Token");
        }

        var user = await _tokenService.GetUserFromRefreshToken(refreshToken);
        if (user == null)
        {
            return Unauthorized("Invalid Refresh Token");
        }

        if (dto == null)
            return BadRequest("Request body is null.");

        if (dto.IngredientGroups == null)
            return BadRequest("IngredientGroups cannot be null.");

        if (dto.Steps == null)
            return BadRequest("Steps cannot be null.");

        // Debug: check user.Id and dto contents here
        if (user.Id == null)
            return Unauthorized("User ID is null.");

        if (string.IsNullOrEmpty(dto.Name))
            return BadRequest("Name cannot be empty.");

        // At this point, dto and user.Id are definitely not null

        var recipe = await _recipeService.CreateRecipe(dto, user.Id);

        return Ok(recipe);
    }
}