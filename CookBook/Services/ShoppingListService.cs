using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.Data;
using CookBook.DTOs;
using CookBook.DTOs.ShoppingList.Request;
using CookBook.DTOs.ShoppingList.Response;
using CookBook.Helpers;
using CookBook.Interfaces;
using CookBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ShoppingListService(ApplicationDBContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResponseDto<ShoppingListResponseDto>> CreateShoppingList(CreateShoppingListDto dto, string userId)
        {
            var shoppingList = new ShoppingList
            {
                Name = dto.Name,
                DateCreated = DateTime.UtcNow,
                IsCompleted = false,
                UserId = userId,
                CompletedOn = null
            };

            _context.ShoppingLists.Add(shoppingList);
            await _context.SaveChangesAsync();

            var responseDto = _mapper.Map<ShoppingListResponseDto>(shoppingList);
            return ServiceResponseHelper.CreateSuccessResponse(responseDto);
        }

        public async Task<ServiceResponseDto<ShoppingListResponseDto>> GetShoppingListById(int id, string userId)
        {
            var shoppingList = await _context.ShoppingLists
                .Where(sl => sl.Id == id && sl.UserId == userId)
                .FirstOrDefaultAsync();

            if (shoppingList == null)
            {
                return ServiceResponseHelper.CreateErrorResponse<ShoppingListResponseDto>("Shopping list not found.");
            }

            var responseDto = _mapper.Map<ShoppingListResponseDto>(shoppingList);
            return ServiceResponseHelper.CreateSuccessResponse(responseDto);
        }

        public async Task<ServiceResponseDto<List<ShoppingListResponseDto>>> GetAllShoppingLists(string userId)
        {
            var shoppingLists = await _context.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .ToListAsync();

            var responseDtos = _mapper.Map<List<ShoppingListResponseDto>>(shoppingLists);

            return ServiceResponseHelper.CreateSuccessResponse(responseDtos);
        }

        public async Task<ServiceResponseDto<bool>> DeleteShoppingList(int id, string userId)
        {
            var shoppingList = await _context.ShoppingLists
                .Where(sl => sl.Id == id && sl.UserId == userId)
                .FirstOrDefaultAsync();

            if (shoppingList == null)
            {
                return ServiceResponseHelper.CreateErrorResponse<bool>("Shopping list not found.");
            }

            _context.ShoppingLists.Remove(shoppingList);
            await _context.SaveChangesAsync();

            return ServiceResponseHelper.CreateSuccessResponse(true);
        }

        public async Task<ServiceResponseDto<ShoppingListResponseDto>> UpdateShoppingList(int id, UpdateShoppingListDto dto, string userId)
        {
            var shoppingList = await _context.ShoppingLists
                .Where(sl => sl.Id == id && sl.UserId == userId)
                .FirstOrDefaultAsync();

            if (shoppingList == null)
            {
                return ServiceResponseHelper.CreateErrorResponse<ShoppingListResponseDto>("Shopping list not found.");
            }

            if (dto.Name != null)
            {
                shoppingList.Name = dto.Name;
            }

            if (dto.IsCompleted.HasValue)
            {
                shoppingList.IsCompleted = dto.IsCompleted.Value;
                shoppingList.CompletedOn = dto.IsCompleted.Value ? DateTime.UtcNow : (DateTime?)null;
            }

            await _context.SaveChangesAsync();

            var responseDto = _mapper.Map<ShoppingListResponseDto>(shoppingList);
            return ServiceResponseHelper.CreateSuccessResponse(responseDto);
        }
        
        public async Task<ServiceResponseDto<ShoppingListItemResponseDto>> AddItemToShoppingList(int shoppingListId, CreateShoppingListItemDto dto, string userId)
        {
            var shoppingList = await _context.ShoppingLists
                .Where(sl => sl.Id == shoppingListId && sl.UserId == userId)
                .FirstOrDefaultAsync();

            if (shoppingList == null)
            {
                return ServiceResponseHelper.CreateErrorResponse<ShoppingListItemResponseDto>("Shopping list not found.");
            }

            var shoppingListItem = new ShoppingListItem
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Unit = dto.Unit,
                IsPurchased = false,
                ShoppingListId = shoppingListId
            };

            _context.ShoppingListItems.Add(shoppingListItem);
            await _context.SaveChangesAsync();

            var responseDto = _mapper.Map<ShoppingListItemResponseDto>(shoppingListItem);
            return ServiceResponseHelper.CreateSuccessResponse(responseDto);
        }
    }

}