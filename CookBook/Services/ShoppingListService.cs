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
    }
}