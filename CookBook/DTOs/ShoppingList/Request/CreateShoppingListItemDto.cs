using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.ShoppingList.Request
{
    public class CreateShoppingListItemDto
    {
        public required string Name { get; set; }
        public bool IsPurchased { get; set; } = false;
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}