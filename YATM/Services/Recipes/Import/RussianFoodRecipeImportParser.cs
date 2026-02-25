using YATM.Core.Attributes;
using HtmlAgilityPack;

namespace YATM.Services.Recipes.Import
{
    [Injectable]
    [Injectable(typeof(IRecipeImportParser))]
    public class RussianFoodRecipeImportParser : RecipeImportParserBase
    {
        private static readonly IReadOnlyCollection<string> _supportedHosts = new[] { "russianfood.com" };

        public override string SiteDisplayName => "RussianFood";

        public override IReadOnlyCollection<string> SupportedHosts => _supportedHosts;

        protected override RecipeImportPayload? ParseCore(string html, Uri recipeUri)
        {
            var fromGenericParser = base.ParseCore(html, recipeUri);
            if (fromGenericParser != null && (fromGenericParser.Ingredients.Any() || fromGenericParser.Steps.Any()))
                return fromGenericParser;

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var title = document.DocumentNode.SelectSingleNode("//h1")?.InnerText;
            if (string.IsNullOrWhiteSpace(title))
                return fromGenericParser;

            var description = document.DocumentNode
                .SelectSingleNode("//meta[@name='description' or @name='Description']")
                ?.GetAttributeValue("content", null);

            var ingredients = new List<string>();
            var ingredientNodes = document.DocumentNode
                .SelectNodes("//table[contains(@class,'ingr')]//tr[contains(@class,'ingr_tr_')]//span");

            if (ingredientNodes != null)
            {
                foreach (var ingredientNode in ingredientNodes)
                {
                    var text = ingredientNode.InnerText?.Trim();
                    if (string.IsNullOrWhiteSpace(text) || text == "*")
                        continue;

                    ingredients.Add(text);
                }
            }

            var steps = new List<string>();
            var stepNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'step_n')]/p");
            if (stepNodes != null)
            {
                foreach (var stepNode in stepNodes)
                {
                    var text = stepNode.InnerText?.Trim();
                    if (string.IsNullOrWhiteSpace(text))
                        continue;

                    steps.Add(text);
                }
            }

            return new RecipeImportPayload(title, description, ingredients, steps);
        }
    }
}
