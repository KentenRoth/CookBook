using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookBook.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;
[ApiController]
[Route("api/favorite")]
public class FavoriteRecipeController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoriteRecipeController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpPost("{recipeId}")]
    public async Task<IActionResult> AddToFavorites(int recipeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _favoriteService.AddToFavorites(recipeId, userId);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
}