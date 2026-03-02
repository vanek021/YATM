using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YATM.BlazorModels.Recipes;
using YATM.Data;
using YATM.Models.Entities;
using YATM.Models.Entities.Recipes;
using YATM.Services.Recipes.Import;

namespace YATM.Services
{
    public class RecipeService
    {
        private readonly ApplicationContext _appCtx;
        private readonly Database _db;
        private readonly IMapper _mapper;
        private readonly RecipeImportService _recipeImportService;

        public RecipeService(ApplicationContext appCtx, Database db, IMapper mapper, RecipeImportService recipeImportService)
        {
            _appCtx = appCtx;
            _db = db;
            _mapper = mapper;
            _recipeImportService = recipeImportService;
        }

        public async Task<List<RecipeBlazorModel>> GetRecipesByUserAsync(User user)
        {
            var recipes = await _db.Recipes.GetAllByUserAsync(user.Id);
            return _mapper.Map<List<RecipeBlazorModel>>(recipes);
        }

        public List<RecipeImportSourceSiteBlazorModel> GetSupportedImportSites()
        {
            return _recipeImportService.GetSupportedSites()
                .Select(site => new RecipeImportSourceSiteBlazorModel
                {
                    Code = site.Code,
                    Name = site.Name,
                    Hosts = site.Hosts,
                })
                .ToList();
        }

        public async Task<Recipe> CreateRecipeAsync(RecipeBlazorModel model)
        {
            var recipe = _mapper.Map<Recipe>(model);
            recipe.UserId = _appCtx.CurrentUser.Id;
            _db.Recipes.Insert(recipe);
            await _db.SaveChangesAsync();
            return recipe;
        }

        public async Task<RecipeBlazorModel> ImportRecipeAsync(string url, CancellationToken cancellationToken = default)
        {
            var importedRecipe = await _recipeImportService.ImportFromUrlAsync(url, cancellationToken);
            var sourceSite = await EnsureSourceSiteAsync(importedRecipe);

            var recipe = new Recipe
            {
                Title = importedRecipe.Title,
                Content = importedRecipe.ContentHtml,
                UserId = _appCtx.CurrentUser.Id,
                SourceSiteId = sourceSite.Id,
                SourceUrl = importedRecipe.SourceUrl,
            };

            _db.Recipes.Insert(recipe);
            await _db.SaveChangesAsync();

            recipe.SourceSite = sourceSite;
            return _mapper.Map<RecipeBlazorModel>(recipe);
        }

        public async Task UpdateRecipeAsync(RecipeBlazorModel model)
        {
            if (model.Id == default)
                throw new ArgumentException();

            var recipe = await _db.Recipes.GetByIdAsync(model.Id, _appCtx.CurrentUser.Id);

            if (recipe is null)
                return;

            _mapper.Map(model, recipe);
            _db.Recipes.Update(recipe);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(long id)
        {
            var recipe = await _db.Recipes.GetByIdAsync(id, _appCtx.CurrentUser.Id);

            if (recipe is null)
                return;

            _db.Recipes.Delete(recipe);
            await _db.SaveChangesAsync();
        }

        private async Task<RecipeSourceSite> EnsureSourceSiteAsync(ImportedRecipeModel importedRecipe)
        {
            var existing = await _db.RecipeSourceSites.GetByCodeAsync(importedRecipe.SourceCode);
            if (existing != null)
            {
                var hasChanges = existing.Name != importedRecipe.SourceName
                    || existing.Host != importedRecipe.SourceHost;

                if (hasChanges)
                {
                    existing.Name = importedRecipe.SourceName;
                    existing.Host = importedRecipe.SourceHost;
                    _db.RecipeSourceSites.Update(existing);
                    await _db.SaveChangesAsync();
                }

                return existing;
            }

            var sourceSite = new RecipeSourceSite
            {
                Code = importedRecipe.SourceCode,
                Name = importedRecipe.SourceName,
                Host = importedRecipe.SourceHost,
            };

            _db.RecipeSourceSites.Insert(sourceSite);
            try
            {
                await _db.SaveChangesAsync();
                return sourceSite;
            }
            catch (DbUpdateException)
            {
                var createdByAnotherRequest = await _db.RecipeSourceSites.GetByCodeAsync(importedRecipe.SourceCode);
                if (createdByAnotherRequest != null)
                    return createdByAnotherRequest;

                throw;
            }
        }
    }
}
