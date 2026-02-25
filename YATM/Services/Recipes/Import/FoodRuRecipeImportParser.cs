using YATM.Core.Attributes;

namespace YATM.Services.Recipes.Import
{
    [Injectable]
    [Injectable(typeof(IRecipeImportParser))]
    public class FoodRuRecipeImportParser : RecipeImportParserBase
    {
        private static readonly IReadOnlyCollection<string> _supportedHosts = new[] { "food.ru" };

        public override string SiteDisplayName => "Food.ru";

        public override IReadOnlyCollection<string> SupportedHosts => _supportedHosts;
    }
}
