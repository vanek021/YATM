using System.Net;

namespace YATM.Services.Recipes.Import
{
    public class RecipeImportPayload
    {
        public RecipeImportPayload(string title, string? description, IEnumerable<string>? ingredients, IEnumerable<string>? steps)
        {
            Title = NormalizeText(title) ?? string.Empty;
            Description = NormalizeText(description);
            Ingredients = NormalizeItems(ingredients);
            Steps = NormalizeItems(steps);
        }

        public string Title { get; }

        public string? Description { get; }

        public List<string> Ingredients { get; }

        public List<string> Steps { get; }

        private static List<string> NormalizeItems(IEnumerable<string>? items)
        {
            if (items == null)
                return new();

            return items
                .Select(NormalizeText)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Cast<string>()
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static string? NormalizeText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var decoded = WebUtility.HtmlDecode(value)
                .Replace('\u00A0', ' ')
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\t", " ");

            var compact = string.Join(" ", decoded.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            return string.IsNullOrWhiteSpace(compact) ? null : compact.Trim();
        }
    }
}
