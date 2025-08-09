using AutoMapper;
using CookBook.DTOs.Recipes.Response;
using CookBook.Models;

namespace CookBook.Mappers;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<Recipe, RecipeResponseDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.DateCreated));
        
        CreateMap<Recipe, RecipeResponseDto>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)));

        CreateMap<RecipeStep, RecipeStepResponseDto>();
        CreateMap<IngredientGroup, IngredientGroupResponseDto>();
        CreateMap<Ingredient, IngredientResponseDto>();
    }
}