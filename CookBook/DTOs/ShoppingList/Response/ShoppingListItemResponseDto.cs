using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.ShoppingList.Response
{
    public class ShoppingListItemResponseDto
    {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public bool IsPurchased { get; set; }
    }
}