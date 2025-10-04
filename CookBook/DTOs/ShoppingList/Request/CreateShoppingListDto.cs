using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.ShoppingList.Request
{
    public class CreateShoppingListDto
    {
        public string Name { get; set; } = string.Empty;
    }
}