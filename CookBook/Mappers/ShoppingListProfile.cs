using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CookBook.Mappers
{
    public class ShoppingListProfile : Profile
    {
        public ShoppingListProfile()
        {
            CreateMap<Models.ShoppingList, DTOs.ShoppingList.Response.ShoppingListResponseDto>();
            CreateMap<Models.ShoppingListItem, DTOs.ShoppingList.Response.ShoppingListItemResponseDto>();
        }
    }
}