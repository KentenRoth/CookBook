using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.Recipes.Request
{
    public class UpdateIngredientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
    }
}