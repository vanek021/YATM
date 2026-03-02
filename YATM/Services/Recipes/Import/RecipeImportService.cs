using System.Text;
using System.Text.RegularExpressions;

namespace YATM.Services.Recipes.Import
{
    public class RecipeImportService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly List<IRecipeSiteParser> _parsers;

        public RecipeImportService(IHttpClientFactory httpClientFactory, IEnumerable<IRecipeSiteParser> parsers)
        {
            _httpClientFactory = httpClientFactory;
            _parsers = parsers.ToList();
        }

        public List<RecipeImportSiteDescriptor> GetSupportedSites()
        {
            return _parsers
                .GroupBy(p => p.SourceCode)
                .Select(g => new RecipeImportSiteDescriptor
                {
                    Code = g.Key,
                    Name = g.First().SourceName,
                    Hosts = g.SelectMany(p => p.SupportedHosts)
                        .Select(RecipeImportParserHelpers.NormalizeHost)
                        .Distinct()
                        .OrderBy(h => h)
                        .ToList(),
                })
                .OrderBy(site => site.Name)
                .ToList();
        }

        public async Task<ImportedRecipeModel> ImportFromUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("Нужно указать ссылку на рецепт.");

            if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
                throw new InvalidOperationException("Невалидная ссылка. Укажите полный URL (https://...).");

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                throw new InvalidOperationException("Поддерживаются только HTTP/HTTPS ссылки.");

            var parser = _parsers.FirstOrDefault(p => p.CanParse(uri));
            if (parser is null)
            {
                var hosts = GetSupportedSites()
                    .SelectMany(site => site.Hosts)
                    .Distinct()
                    .OrderBy(host => host);

                throw new InvalidOperationException($"Сайт не поддерживается. Поддерживаемые домены: {string.Join(", ", hosts)}");
            }

            var html = await DownloadHtmlAsync(uri, cancellationToken);
            var importedRecipe = parser.Parse(uri, html);

            if (string.IsNullOrWhiteSpace(importedRecipe.Title) || string.IsNullOrWhiteSpace(importedRecipe.ContentHtml))
                throw new InvalidOperationException("Не удалось извлечь рецепт с указанной страницы.");

            return importedRecipe;
        }

        private async Task<string> DownloadHtmlAsync(Uri uri, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("recipe-import");
            client.Timeout = TimeSpan.FromSeconds(30);

            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; YATMRecipeImporter/1.0)");

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Не удалось открыть страницу рецепта. HTTP {(int)response.StatusCode}.");

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var charset = response.Content.Headers.ContentType?.CharSet;

            return DecodeHtml(bytes, charset);
        }

        private static string DecodeHtml(byte[] contentBytes, string? charset)
        {
            var normalizedCharset = NormalizeCharset(charset);
            if (string.IsNullOrWhiteSpace(normalizedCharset))
                normalizedCharset = ExtractCharsetFromHtml(contentBytes);

            if (normalizedCharset == "windows-1251" || normalizedCharset == "cp1251")
                return RecipeImportParserHelpers.DecodeWindows1251(contentBytes);

            if (!string.IsNullOrWhiteSpace(normalizedCharset))
            {
                try
                {
                    return Encoding.GetEncoding(normalizedCharset).GetString(contentBytes);
                }
                catch
                {
                    // Ignored intentionally, fallback to UTF-8 below.
                }
            }

            return Encoding.UTF8.GetString(contentBytes);
        }

        private static string NormalizeCharset(string? charset)
        {
            if (string.IsNullOrWhiteSpace(charset))
                return string.Empty;

            return charset.Trim().Trim('\'', '"').ToLowerInvariant();
        }

        private static string ExtractCharsetFromHtml(byte[] contentBytes)
        {
            var probeLength = Math.Min(contentBytes.Length, 8192);
            var head = Encoding.ASCII.GetString(contentBytes, 0, probeLength);
            var match = Regex.Match(head, "charset\\s*=\\s*[\"']?(?<charset>[A-Za-z0-9_\\-]+)", RegexOptions.IgnoreCase);

            return match.Success
                ? NormalizeCharset(match.Groups["charset"].Value)
                : string.Empty;
        }
    }
}
