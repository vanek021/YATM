using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YATM.Core.Models;

namespace YATM.Models.Entities.Recipes
{
    public class Recipe : BaseRecord
    {
        [Required]
        [MaxLength(512)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Ingredients { get; set; } = string.Empty;

        public string Steps { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string SourceRecipeUrl { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public long UserId { get; set; }

        [ForeignKey(nameof(RecipeSourceId))]
        public RecipeSource RecipeSource { get; set; } = null!;
        public long RecipeSourceId { get; set; }
    }
}
