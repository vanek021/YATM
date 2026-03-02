namespace YATM.Services.Recipes.Import
{
    public class RussianFoodRecipeSiteParser : IRecipeSiteParser
    {
        public string SourceCode => "russianfood";
        public string SourceName => "RussianFood";
        public IReadOnlyCollection<string> SupportedHosts { get; } = new List<string>
        {
            "russianfood.com",
            "www.russianfood.com",
        };

        public bool CanParse(Uri uri)
        {
            return RecipeImportParserHelpers.HostMatches(uri, SupportedHosts);
        }

        public ImportedRecipeModel Parse(Uri uri, string html)
        {
            var title = RecipeImportParserHelpers.CleanText(
                RecipeImportParserHelpers.GetFirstMatchGroup(html, "<h1[^>]*>(.*?)</h1>") ?? string.Empty);

            var ingredients = RecipeImportParserHelpers.GetAllMatches(
                html,
                "<tr[^>]*class=[\"']ingr_tr_[^\"']*[\"'][^>]*>(.*?)</tr>");

            var steps = RecipeImportParserHelpers.GetAllMatches(
                html,
                "<div[^>]*class=[\"'][^\"']*step_n[^\"']*[\"'][^>]*>.*?<p>(.*?)</p>");

            if (ingredients.Count == 0 || steps.Count == 0)
                throw new InvalidOperationException("Страница RussianFood не содержит полноценного рецепта для импорта.");

            var content = RecipeImportParserHelpers.BuildContentHtml(null, ingredients, steps);

            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidOperationException("Не удалось определить заголовок рецепта на странице RussianFood.");

            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Не удалось извлечь шаги или ингредиенты рецепта на странице RussianFood.");

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
    }
}
