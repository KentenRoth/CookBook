using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookBook.DTOs.ShoppingList.Request;
using CookBook.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers
{
    [ApiController]
    [Route("api/shopping")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpPost("lists")]
        public async Task<IActionResult> CreateShoppingList(CreateShoppingListDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var shoppingList = await _shoppingListService.CreateShoppingList(dto, userId);

            return Ok(shoppingList);
        }

        [HttpGet("lists/{id}")]
        public async Task<IActionResult> GetShoppingListById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var shoppingList = await _shoppingListService.GetShoppingListById(id, userId);

            if (!shoppingList.Success)
            {
                return NotFound(shoppingList);
            }

            return Ok(shoppingList);
        }
    }
}