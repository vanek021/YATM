namespace YATM.Services.Recipes.Import
{
    public abstract class RecipeImportParserBase : IRecipeImportParser
    {
        public abstract string SiteDisplayName { get; }

        public abstract IReadOnlyCollection<string> SupportedHosts { get; }

        public bool CanParse(Uri recipeUri)
        {
            var host = NormalizeHost(recipeUri.Host);
            return SupportedHosts.Any(supportedHost => IsHostMatch(host, NormalizeHost(supportedHost)));
        }

        public RecipeImportPayload Parse(string html, Uri recipeUri)
        {
            var payload = ParseCore(html, recipeUri);
            if (payload == null || string.IsNullOrWhiteSpace(payload.Title))
                throw new InvalidOperationException($"Не удалось извлечь рецепт со страницы '{recipeUri.Host}'.");

            return payload;
        }

        protected virtual RecipeImportPayload? ParseCore(string html, Uri recipeUri)
        {
            return RecipeMarkupExtractor.Extract(html);
        }

        protected static string NormalizeHost(string host)
        {
            var normalized = host.Trim().ToLowerInvariant();
            return normalized.StartsWith("www.", StringComparison.OrdinalIgnoreCase)
                ? normalized[4..]
                : normalized;
        }

        private static bool IsHostMatch(string host, string supportedHost)
        {
            return host.Equals(supportedHost, StringComparison.OrdinalIgnoreCase)
                   || host.EndsWith($".{supportedHost}", StringComparison.OrdinalIgnoreCase);
        }
    }
}
