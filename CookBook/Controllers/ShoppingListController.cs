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

        [HttpGet("lists")]
        public async Task<IActionResult> GetAllShoppingLists()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var shoppingLists = await _shoppingListService.GetAllShoppingLists(userId);

            return Ok(shoppingLists);
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

        [HttpDelete("lists/{id}")]
        public async Task<IActionResult> DeleteShoppingList(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _shoppingListService.DeleteShoppingList(id, userId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPut("lists/{id}")]
        public async Task<IActionResult> UpdateShoppingList(int id, UpdateShoppingListDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var shoppingList = await _shoppingListService.UpdateShoppingList(id, dto, userId);

            if (!shoppingList.Success)
            {
                return NotFound(shoppingList);
            }

            return Ok(shoppingList);
        }

        [HttpPost("lists/{shoppingListId}/items")]
        public async Task<IActionResult> AddItemToShoppingList(int shoppingListId, CreateShoppingListItemDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _shoppingListService.AddItemToShoppingList(shoppingListId, dto, userId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromShoppingList(int itemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _shoppingListService.RemoveItemFromShoppingList(itemId, userId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateShoppingListItem(int itemId, UpdateShoppingListItemDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _shoppingListService.UpdateShoppingListItem(itemId, dto, userId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}