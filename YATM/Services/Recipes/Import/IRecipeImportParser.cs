namespace YATM.Services.Recipes.Import
{
    public interface IRecipeImportParser
    {
        string SiteDisplayName { get; }

        IReadOnlyCollection<string> SupportedHosts { get; }

        bool CanParse(Uri recipeUri);

        RecipeImportPayload Parse(string html, Uri recipeUri);
    }
}
