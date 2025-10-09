using AutoMapper;
using CookBook.DTOs.ShoppingList.Response;

using CookBook.Models;

namespace CookBook.Mappers
{
    public class ShoppingListProfile : Profile
    {
        public ShoppingListProfile()
        {
            CreateMap<ShoppingList, ShoppingListResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<ShoppingListItem, ShoppingListItemResponseDto>();
        }
    }
}