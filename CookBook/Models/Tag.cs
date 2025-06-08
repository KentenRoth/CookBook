using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class Tag
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
 
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}