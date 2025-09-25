using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.Recipes.Request
{
    public class UpdateRecipeStepDto
    {
        public int StepNumber { get; set; }
        public string Instruction { get; set; } = string.Empty;
    }
}