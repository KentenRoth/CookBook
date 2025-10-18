using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBook.DTOs;
using CookBook.DTOs.ShoppingList.Request;
using CookBook.DTOs.ShoppingList.Response;

namespace CookBook.Interfaces
{
    public interface IShoppingListService
    {
        Task<ServiceResponseDto<ShoppingListResponseDto>> CreateShoppingList(CreateShoppingListDto dto, string userId);
        Task<ServiceResponseDto<ShoppingListResponseDto>> GetShoppingListById(int id, string userId);
        Task<ServiceResponseDto<List<ShoppingListResponseDto>>> GetAllShoppingLists(string userId);
        Task<ServiceResponseDto<bool>> DeleteShoppingList(int id, string userId);
        Task<ServiceResponseDto<ShoppingListResponseDto>> UpdateShoppingList(int id, UpdateShoppingListDto dto, string userId);
        Task<ServiceResponseDto<ShoppingListItemResponseDto>> AddItemToShoppingList(int shoppingListId, CreateShoppingListItemDto dto, string userId);
        Task<ServiceResponseDto<bool>> RemoveItemFromShoppingList(int itemId, string userId);
        Task<ServiceResponseDto<ShoppingListItemResponseDto>> UpdateShoppingListItem(int itemId, UpdateShoppingListItemDto dto, string userId);
    }
}