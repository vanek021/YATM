using System.ComponentModel.DataAnnotations;
using YATM.Core.Models;

namespace YATM.Models.Entities.Recipes
{
    public class RecipeSource : BaseRecord
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Host { get; set; } = string.Empty;

        public virtual List<Recipe> Recipes { get; set; } = new();
    }
}
