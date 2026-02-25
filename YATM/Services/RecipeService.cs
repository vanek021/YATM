using AutoMapper;
using YATM.BlazorModels.Recipes;
using YATM.Data;
using YATM.Models.Entities;
using YATM.Models.Entities.Recipes;
using YATM.Services.Recipes.Import;

namespace YATM.Services
{
    public class RecipeService
    {
        private readonly Database _db;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IReadOnlyCollection<IRecipeImportParser> _parsers;

        public RecipeService(Database db, IMapper mapper, IHttpClientFactory httpClientFactory, IEnumerable<IRecipeImportParser> parsers)
        {
            _db = db;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _parsers = parsers.ToList();
        }

        public async Task<List<RecipeBlazorModel>> GetRecipesByUserAsync(User user)
        {
            var recipes = await _db.Recipes.GetByUserAsync(user.Id);
            return _mapper.Map<List<RecipeBlazorModel>>(recipes);
        }

        public List<RecipeImportSiteBlazorModel> GetSupportedSites()
        {
            return _parsers
                .SelectMany(parser => parser.SupportedHosts.Select(host => new RecipeImportSiteBlazorModel
                {
                    Name = parser.SiteDisplayName,
                    Host = NormalizeHost(host)
                }))
                .GroupBy(site => site.Host, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .OrderBy(site => site.Name)
                .ThenBy(site => site.Host)
                .ToList();
        }

        public async Task<RecipeBlazorModel> ImportRecipeFromUrlAsync(User user, string recipeUrl, CancellationToken cancellationToken = default)
        {
            if (_parsers.Count == 0)
                throw new InvalidOperationException("Не зарегистрировано ни одного парсера рецептов.");

            if (string.IsNullOrWhiteSpace(recipeUrl))
                throw new ArgumentException("Укажите ссылку на рецепт.");

            if (!Uri.TryCreate(recipeUrl, UriKind.Absolute, out var recipeUri)
                || (recipeUri.Scheme != Uri.UriSchemeHttp && recipeUri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("Ссылка должна начинаться с http:// или https://.");
            }

            var parser = _parsers.FirstOrDefault(e => e.CanParse(recipeUri));
            if (parser == null)
                throw new ArgumentException($"Сайт '{NormalizeHost(recipeUri.Host)}' пока не поддерживается.");

            var normalizedUrl = NormalizeUrl(recipeUri);
            var pageHtml = await DownloadPageHtmlAsync(recipeUri, cancellationToken);
            var payload = parser.Parse(pageHtml, recipeUri);

            if (!payload.Ingredients.Any() && !payload.Steps.Any())
                throw new InvalidOperationException("Не удалось извлечь ингредиенты и шаги приготовления.");

            var source = await GetOrCreateRecipeSourceAsync(parser);
            var existingRecipe = await _db.Recipes.GetByUserAndSourceUrlAsync(user.Id, normalizedUrl);

            var recipe = existingRecipe ?? new Recipe
            {
                UserId = user.Id,
                SourceRecipeUrl = normalizedUrl
            };

            recipe.Title = payload.Title;
            recipe.Description = payload.Description;
            recipe.Ingredients = string.Join('\n', payload.Ingredients);
            recipe.Steps = string.Join('\n', payload.Steps);
            recipe.SourceRecipeUrl = normalizedUrl;
            recipe.RecipeSourceId = source.Id;

            _db.Recipes.InsertOrUpdate(recipe);
            await _db.SaveChangesAsync();

            var savedRecipe = await _db.Recipes.GetByIdForUserAsync(recipe.Id, user.Id);
            if (savedRecipe == null)
                throw new InvalidOperationException("Не удалось сохранить рецепт после импорта.");

            return _mapper.Map<RecipeBlazorModel>(savedRecipe);
        }

        private async Task<RecipeSource> GetOrCreateRecipeSourceAsync(IRecipeImportParser parser)
        {
            foreach (var host in parser.SupportedHosts.Select(NormalizeHost))
            {
                var source = await _db.RecipeSources.GetByHostAsync(host);
                if (source != null)
                    return source;
            }

            var primaryHost = NormalizeHost(parser.SupportedHosts.First());
            var newSource = new RecipeSource
            {
                Name = parser.SiteDisplayName,
                Host = primaryHost
            };

            _db.RecipeSources.Insert(newSource);
            await _db.SaveChangesAsync();

            return newSource;
        }

        private async Task<string> DownloadPageHtmlAsync(Uri recipeUri, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(20);

            using var request = new HttpRequestMessage(HttpMethod.Get, recipeUri);
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36");
            request.Headers.AcceptLanguage.ParseAdd("ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Не удалось загрузить страницу рецепта (HTTP {(int)response.StatusCode}).");

            return await response.Content.ReadAsStringAsync();
        }

        private static string NormalizeUrl(Uri uri)
        {
            var builder = new UriBuilder(uri)
            {
                Host = NormalizeHost(uri.Host),
                Fragment = string.Empty
            };

            if (builder.Scheme == Uri.UriSchemeHttp && builder.Port == 80)
                builder.Port = -1;

            if (builder.Scheme == Uri.UriSchemeHttps && builder.Port == 443)
                builder.Port = -1;

            var leftPart = builder.Uri.GetLeftPart(UriPartial.Path).TrimEnd('/');
            if (string.IsNullOrWhiteSpace(leftPart))
                leftPart = builder.Uri.GetLeftPart(UriPartial.Authority);

            return string.IsNullOrWhiteSpace(builder.Query)
                ? leftPart
                : $"{leftPart}{builder.Query}";
        }

        private static string NormalizeHost(string host)
        {
            var normalized = host.Trim().ToLowerInvariant();
            return normalized.StartsWith("www.", StringComparison.OrdinalIgnoreCase)
                ? normalized[4..]
                : normalized;
        }
    }
}
