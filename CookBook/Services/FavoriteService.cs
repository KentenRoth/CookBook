using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.Data;
using CookBook.DTOs;
using CookBook.DTOs.Recipes.Response;
using CookBook.Helpers;
using CookBook.Interfaces;
using CookBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services;

public class FavoriteService : IFavoriteService
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public FavoriteService(ApplicationDBContext context, UserManager<AppUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ServiceResponseDto<bool>> AddToFavorites(int recipeId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<bool>("User not found");
        }

        var recipe = await _context.Recipes.FindAsync(recipeId);
        if (recipe == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<bool>("Recipe not found");
        }

        var existingFavorite = await _context.FavoriteRecipes
            .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);
        if (existingFavorite != null)
        {
            return ServiceResponseHelper.CreateErrorResponse<bool>("Recipe already in favorites");
        }

        var favorite = new FavoriteRecipe
        {
            UserId = userId,
            RecipeId = recipeId,
        };

        _context.FavoriteRecipes.Add(favorite);
        await _context.SaveChangesAsync();

        return ServiceResponseHelper.CreateSuccessResponse(true);
    }

    public async Task<ServiceResponseDto<List<RecipeResponseDto>>> GetFavoriteRecipes(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<List<RecipeResponseDto>>("User not found");
        }

        var favoriteRecipes = await _context.FavoriteRecipes
            .Where(f => f.UserId == userId)
            .Include(f => f.Recipe)
            .ThenInclude(r => r.Tags)
            .Include(f => f.Recipe)
            .ThenInclude(r => r.IngredientGroups)
            .ThenInclude(ig => ig.Ingredients)
            .Include(f => f.Recipe)
            .ThenInclude(r => r.RecipeSteps)
            .ToListAsync();

        var recipeDtos = favoriteRecipes.Select(f => _mapper.Map<RecipeResponseDto>(f.Recipe)).ToList();

        return ServiceResponseHelper.CreateSuccessResponse(recipeDtos);
    }
    
    public async Task<ServiceResponseDto<bool>> RemoveFromFavorites(int recipeId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<bool>("User not found");
        }

        var favorite = await _context.FavoriteRecipes
            .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);
        if (favorite == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<bool>("Recipe not in favorites");
        }

        _context.FavoriteRecipes.Remove(favorite);
        await _context.SaveChangesAsync();

        return ServiceResponseHelper.CreateSuccessResponse(true);
    }
}