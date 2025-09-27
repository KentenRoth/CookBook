using AutoMapper;
using CookBook.DTOs.Recipes.Response;
using CookBook.Models;

namespace CookBook.Mappers;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
    CreateMap<Recipe, RecipeResponseDto>()
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.DateCreated))
        .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.User.Name))
        .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.RecipeSteps));


    CreateMap<RecipeStep, RecipeStepResponseDto>();
    CreateMap<IngredientGroup, IngredientGroupResponseDto>();
    CreateMap<Ingredient, IngredientResponseDto>();
    }
}