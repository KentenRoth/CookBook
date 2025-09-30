using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool IsPurchased { get; set; } = false;
        public int ShoppingListId { get; set; }
        public ShoppingList ShoppingList { get; set; } = null!;
    }
}