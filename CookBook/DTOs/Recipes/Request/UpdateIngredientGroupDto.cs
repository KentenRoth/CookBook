using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.Recipes.Request
{
    public class UpdateIngredientGroupDto
    {
        public string Name { get; set; } = string.Empty;
        public List<UpdateIngredientDto> Ingredients { get; set; } = new();
    }
}