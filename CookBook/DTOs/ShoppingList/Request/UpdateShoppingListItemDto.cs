using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.ShoppingList.Request
{
    public class UpdateShoppingListItemDto
    {
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public string? Unit { get; set; }
        public bool? IsPurchased { get; set; }
    }
}