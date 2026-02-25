using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YATM.BlazorModels.Recipes
{
    public class RecipeBlazorModel
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Ingredients { get; set; } = string.Empty;

        public string Steps { get; set; } = string.Empty;

        public string SourceRecipeUrl { get; set; } = string.Empty;

        public string SourceName { get; set; } = string.Empty;

        public string SourceHost { get; set; } = string.Empty;

        public DateTime ImportedAt { get; set; }

        [NotMapped]
        public List<string> IngredientItems => SplitLines(Ingredients);

        [NotMapped]
        public List<string> StepItems => SplitLines(Steps);

        private static List<string> SplitLines(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new();

            return value
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToList();
        }
    }
}
