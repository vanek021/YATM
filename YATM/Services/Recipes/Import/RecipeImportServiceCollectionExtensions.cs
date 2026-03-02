using Microsoft.Extensions.DependencyInjection;

namespace YATM.Services.Recipes.Import
{
    public static class RecipeImportServiceCollectionExtensions
    {
        public static IServiceCollection AddRecipeImporting(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<RecipeImportService>();

            var parserTypes = typeof(IRecipeSiteParser).Assembly.GetTypes()
                .Where(type =>
                    !type.IsAbstract
                    && !type.IsInterface
                    && typeof(IRecipeSiteParser).IsAssignableFrom(type));

            foreach (var parserType in parserTypes)
                services.AddScoped(typeof(IRecipeSiteParser), parserType);

            return services;
        }
    }
}
