using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? CompletedOn { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; }
        public ICollection<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>();
    }
}