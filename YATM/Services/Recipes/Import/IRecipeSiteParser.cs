namespace YATM.Services.Recipes.Import
{
    public interface IRecipeSiteParser
    {
        string SourceCode { get; }
        string SourceName { get; }
        IReadOnlyCollection<string> SupportedHosts { get; }

        bool CanParse(Uri uri);
        ImportedRecipeModel Parse(Uri uri, string html);
    }
}
