using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.ShoppingList.Request
{
    public class UpdateShoppingListDto
    {
        public string? Name { get; set; }
        public bool? IsCompleted { get; set; }
    }
}