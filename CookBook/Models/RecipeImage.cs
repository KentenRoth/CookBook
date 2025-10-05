using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.Models
{
    public class RecipeImage
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Url { get; set; }
        public bool IsPrimary { get; set; }
        public Recipe Recipe { get; set; }
    }
}