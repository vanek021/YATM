namespace YATM.Services.Recipes.Import
{
    public class FoodRuRecipeSiteParser : IRecipeSiteParser
    {
        public string SourceCode => "foodru";
        public string SourceName => "Food.ru";
        public IReadOnlyCollection<string> SupportedHosts { get; } = new List<string>
        {
            "food.ru",
            "www.food.ru",
        };

        public bool CanParse(Uri uri)
        {
            return RecipeImportParserHelpers.HostMatches(uri, SupportedHosts);
        }

        public ImportedRecipeModel Parse(Uri uri, string html)
        {
            var title = GetTitle(html);
            var description = GetDescription(html);

            var ingredients = RecipeImportParserHelpers.GetAllMatches(
                html,
                "<tr[^>]*(?:itemprop|itemProp)=[\"']recipeIngredient[\"'][^>]*>(.*?)</tr>");

            var steps = RecipeImportParserHelpers.GetAllMatches(
                html,
                "<span[^>]*class=[\"'][^\"']*instruction[^\"']*[\"'][^>]*>(.*?)</span>");

            if (ingredients.Count == 0 || steps.Count == 0)
                throw new InvalidOperationException("Страница Food.ru не содержит полноценного рецепта для импорта.");

            var content = RecipeImportParserHelpers.BuildContentHtml(description, ingredients, steps);

            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidOperationException("Не удалось определить заголовок рецепта на странице Food.ru.");

            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Не удалось извлечь шаги или ингредиенты рецепта на странице Food.ru.");

            return new ImportedRecipeModel
            {
                SourceCode = SourceCode,
                SourceName = SourceName,
                SourceHost = RecipeImportParserHelpers.NormalizeHost(uri.Host),
                SourceUrl = uri.ToString(),
                Title = title,
                ContentHtml = content,
            };
        }

        private static string GetTitle(string html)
        {
            var ogTitle = RecipeImportParserHelpers.GetFirstMatchGroup(
                html,
                "<meta[^>]+property=[\"']og:title[\"'][^>]+content=[\"'](.*?)[\"'][^>]*>");

            if (string.IsNullOrWhiteSpace(ogTitle))
            {
                ogTitle = RecipeImportParserHelpers.GetFirstMatchGroup(
                    html,
                    "<meta[^>]+content=[\"'](.*?)[\"'][^>]+property=[\"']og:title[\"'][^>]*>");
            }

            if (string.IsNullOrWhiteSpace(ogTitle))
            {
                ogTitle = RecipeImportParserHelpers.GetFirstMatchGroup(
                    html,
                    "<h1[^>]*>(.*?)</h1>");
            }

            return RecipeImportParserHelpers.CleanText(ogTitle ?? string.Empty);
        }

        private static string GetDescription(string html)
        {
            var ogDescription = RecipeImportParserHelpers.GetFirstMatchGroup(
                html,
                "<meta[^>]+property=[\"']og:description[\"'][^>]+content=[\"'](.*?)[\"'][^>]*>");

            if (string.IsNullOrWhiteSpace(ogDescription))
            {
                ogDescription = RecipeImportParserHelpers.GetFirstMatchGroup(
                    html,
                    "<meta[^>]+content=[\"'](.*?)[\"'][^>]+property=[\"']og:description[\"'][^>]*>");
            }

            return RecipeImportParserHelpers.CleanText(ogDescription ?? string.Empty);
        }
    }
}
