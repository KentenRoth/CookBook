using AutoMapper;
using CookBook.Data;
using CookBook.DTOs;
using CookBook.DTOs.Recipes.Request;
using CookBook.DTOs.Recipes.Response;
using CookBook.Helpers;
using CookBook.Interfaces;
using CookBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services;

public class RecipeService : IRecipeService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDBContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public RecipeService(UserManager<AppUser> userManager, ApplicationDBContext context, ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<ServiceResponseDto<RecipeResponseDto>> CreateRecipe(CreateRecipeDto dto, string userId)
    {
        var recipe = new Recipe
        {
            Name = dto.Name ?? string.Empty,
            Description = dto.Description ?? string.Empty,
            IsPublic = dto.IsPublic,
            Servings = dto.Servings,
            PrepTimeMinutes = dto.PrepTimeMinutes,
            CookTimeMinutes = dto.CookTimeMinutes,
            About = dto.About ?? string.Empty,
            Notes = dto.Notes ?? string.Empty,
            DateCreated = DateTime.UtcNow,
            UserId = userId,
            RecipeSteps = dto.Steps.Select(s => new RecipeStep
            {
                StepNumber = s.StepNumber,
                Instruction = s.Instruction ?? string.Empty
            }).ToList(),
            IngredientGroups = dto.IngredientGroups.Select(g => new IngredientGroup
            {
                Name = g.Name ?? string.Empty,
                Ingredients = (g.Ingredients ?? new List<CreateIngredientDto>()).Select(i => new Ingredient
                {
                    Name = i.Name ?? string.Empty,
                    Amount = i.Amount ?? string.Empty
                }).ToList()
            }).ToList()
        };

        var tags = new List<Tag>();

        foreach (var tagName in dto.Tags.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());

            if (existingTag != null)
            {
                tags.Add(existingTag);
            }
            else
            {
                tags.Add(new Tag { Name = tagName });
            }
        }

        recipe.Tags = tags;

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        var responseDto = _mapper.Map<RecipeResponseDto>(recipe);

        return ServiceResponseHelper.CreateSuccessResponse<RecipeResponseDto>(responseDto);
    }

    public async Task<ServiceResponseDto<EmptyDto>> DeleteRecipe(int id, string userId)
    {
        var recipe = await _context.Recipes
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        if (recipe == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>("Recipe not found or you do not have permission to delete it.");
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();

        return ServiceResponseHelper.CreateSuccessResponse<EmptyDto>(new EmptyDto());
    }

    public async Task<ServiceResponseDto<List<RecipeResponseDto>>> GetRecipes()
    {
        var recipes = await _context.Recipes
            .Where(r => r.IsPublic == true)
            .Include(r => r.User)
            .Include(r => r.IngredientGroups)
                .ThenInclude(ig => ig.Ingredients)
            .Include(r => r.RecipeSteps)
            .Include(r => r.Tags)
            .ToListAsync();

        var responseDto = _mapper.Map<List<RecipeResponseDto>>(recipes);

        return ServiceResponseHelper.CreateSuccessResponse<List<RecipeResponseDto>>(responseDto);
    }

    public async Task<ServiceResponseDto<RecipeResponseDto>> GetRecipeById(int id, string? userId = null)
    {
        var recipe = await _context.Recipes
            .Include(r => r.User)
            .Include(r => r.IngredientGroups)
                .ThenInclude(ig => ig.Ingredients)
            .Include(r => r.RecipeSteps)
            .Include(r => r.Tags)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<RecipeResponseDto>("Recipe not found.");
        }

        if (!recipe.IsPublic && (userId == null || recipe.UserId != userId))
        {
            return ServiceResponseHelper.CreateErrorResponse<RecipeResponseDto>("Recipe not found or you do not have permission to view it.");
        }

        var responseDto = _mapper.Map<RecipeResponseDto>(recipe);

        return ServiceResponseHelper.CreateSuccessResponse<RecipeResponseDto>(responseDto);
    }

    public async Task<ServiceResponseDto<List<RecipeResponseDto>>> GetMyRecipes(string userId)
    {
        var recipes = await _context.Recipes
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.IngredientGroups)
                .ThenInclude(ig => ig.Ingredients)
            .Include(r => r.RecipeSteps)
            .Include(r => r.Tags)
            .ToListAsync();

        var responseDto = _mapper.Map<List<RecipeResponseDto>>(recipes);

        return ServiceResponseHelper.CreateSuccessResponse<List<RecipeResponseDto>>(responseDto);
    }

    public async Task<ServiceResponseDto<List<RecipeResponseDto>>> GetRecipeByUser(string username)
    {
        var recipes = await _context.Recipes
            .Where(r => r.User.UserName == username)
            .Where(r => r.IsPublic == true)
            .Include(r => r.User)
            .Include(r => r.IngredientGroups)
                .ThenInclude(ig => ig.Ingredients)
            .Include(r => r.RecipeSteps)
            .Include(r => r.Tags)
            .ToListAsync();

        var responseDto = _mapper.Map<List<RecipeResponseDto>>(recipes);

        return ServiceResponseHelper.CreateSuccessResponse<List<RecipeResponseDto>>(responseDto);
    }

    public async Task<ServiceResponseDto<List<RecipeTagResponseDto>>> GetAllTags()
    {
        var tags = await _context.Tags
            .OrderBy(t => t.Name)
            .Select(t => new RecipeTagResponseDto
            {
                Name = t.Name
            })
            .ToListAsync();

        return ServiceResponseHelper.CreateSuccessResponse(tags);
    }

    public async Task<ServiceResponseDto<RecipeResponseDto>> UpdateRecipe(int recipeId, UpdateRecipeRequestDto dto, string userId)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Tags)
            .Include(r => r.RecipeSteps)
            .Include(r => r.IngredientGroups)
                .ThenInclude(ig => ig.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId && r.UserId == userId);

        if (recipe == null)
            return ServiceResponseHelper.CreateErrorResponse<RecipeResponseDto>("Recipe not found or not owned by user.");

        if (dto.Name != null) recipe.Name = dto.Name;
        if (dto.Description != null) recipe.Description = dto.Description;
        if (dto.About != null) recipe.About = dto.About;
        if (dto.Notes != null) recipe.Notes = dto.Notes;
        if (dto.IsPublic.HasValue) recipe.IsPublic = dto.IsPublic.Value;
        if (dto.PrepTimeMinutes.HasValue) recipe.PrepTimeMinutes = dto.PrepTimeMinutes.Value;
        if (dto.CookTimeMinutes.HasValue) recipe.CookTimeMinutes = dto.CookTimeMinutes.Value;
        if (dto.Servings.HasValue) recipe.Servings = dto.Servings.Value;

        if (dto.Tags != null)
        {
            recipe.Tags.Clear();
            var tags = await _context.Tags.Where(t => dto.Tags.Contains(t.Name)).ToListAsync();

            foreach (var tagName in dto.Tags.Except(tags.Select(t => t.Name)))
            {
                var newTag = new Tag { Name = tagName };
                _context.Tags.Add(newTag);
                tags.Add(newTag);
            }

            recipe.Tags = tags;
        }

        if (dto.Steps != null)
        {
            recipe.RecipeSteps.Clear();
            foreach (var stepDto in dto.Steps.OrderBy(s => s.StepNumber))
            {
                recipe.RecipeSteps.Add(new RecipeStep
                {
                    StepNumber = stepDto.StepNumber,
                    Instruction = stepDto.Instruction
                });
            }
        }

        if (dto.IngredientGroups != null)
        {
            recipe.IngredientGroups.Clear();
            foreach (var groupDto in dto.IngredientGroups)
            {
                var group = new IngredientGroup { Name = groupDto.Name };
                foreach (var ingDto in groupDto.Ingredients)
                {
                    group.Ingredients.Add(new Ingredient
                    {
                        Name = ingDto.Name,
                        Amount = ingDto.Amount
                    });
                }
                recipe.IngredientGroups.Add(group);
            }
        }

        await _context.SaveChangesAsync();

        var responseDto = _mapper.Map<RecipeResponseDto>(recipe);
        return ServiceResponseHelper.CreateSuccessResponse(responseDto);
    }

    public async Task<ServiceResponseDto<List<RecipeResponseDto>>> SearchRecipes(string query, string? currentUserId = null)
    {
        if (string.IsNullOrWhiteSpace(query))
            return ServiceResponseHelper.CreateSuccessResponse(new List<RecipeResponseDto>());

        query = query.ToLower();

        var recipes = await _context.Recipes
        .Include(r => r.IngredientGroups)
            .ThenInclude(ig => ig.Ingredients)
        .Include(r => r.RecipeSteps)
        .Include(r => r.Tags)
        .Include(r => r.User)
        .Where(r =>
            (r.IsPublic || r.UserId == currentUserId) &&
            (
                r.Name.ToLower().Contains(query) ||
                r.Description.ToLower().Contains(query) ||
                r.IngredientGroups.Any(ig =>
                    ig.Ingredients.Any(i => i.Name.ToLower().Contains(query))
                )
            )
        )
        .ToListAsync();

        var responseDto = _mapper.Map<List<RecipeResponseDto>>(recipes);
        return ServiceResponseHelper.CreateSuccessResponse(responseDto);
    }

    public async Task<ServiceResponseDto<List<RecipeResponseDto>>> FilterRecipesByTags(
        List<string> tags, string? currentUserId = null)
    {
        if (tags == null || tags.Count == 0)
            return ServiceResponseHelper.CreateSuccessResponse(new List<RecipeResponseDto>());

        tags = tags.Select(t => t.ToLower()).ToList();

        var recipes = await _context.Recipes
            .Include(r => r.IngredientGroups)
                .ThenInclude(ig => ig.Ingredients)
            .Include(r => r.RecipeSteps)
            .Include(r => r.Tags)
            .Include(r => r.User)
            .Where(r =>
                (r.IsPublic || r.UserId == currentUserId) &&
                r.Tags.Any(t => tags.Contains(t.Name.ToLower()))
            )
            .ToListAsync();

        var responseDto = _mapper.Map<List<RecipeResponseDto>>(recipes);
        return ServiceResponseHelper.CreateSuccessResponse(responseDto);
    }
}