using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.ShoppingList.Response
{
    public class ShoppingListResponseDto
    {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedOn { get; set; }

    public List<ShoppingListItemResponseDto> Items { get; set; } = new();
    }

}