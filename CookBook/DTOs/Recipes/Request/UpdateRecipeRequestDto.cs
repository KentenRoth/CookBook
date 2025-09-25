using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.Recipes.Request
{
    public class UpdateRecipeRequestDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? About { get; set; }
        public string? Notes { get; set; }
        public bool? IsPublic { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public decimal? Servings { get; set; }

        public List<string>? Tags { get; set; }
        public List<UpdateRecipeStepDto>? Steps { get; set; }
        public List<UpdateIngredientGroupDto>? IngredientGroups { get; set; }
    }
}