using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBook.DTOs;
using CookBook.DTOs.Recipes.Response;

namespace CookBook.Interfaces
{
    public interface IFavoriteService
    {
        Task<ServiceResponseDto<bool>> AddToFavorites(int recipeId, string userId);
        Task<ServiceResponseDto<List<RecipeResponseDto>>> GetFavoriteRecipes(string userId);
    }
}