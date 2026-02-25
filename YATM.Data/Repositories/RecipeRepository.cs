using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities.Recipes;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<Recipe>))]
    public class RecipeRepository : WriteableRepository<Recipe>
    {
        public RecipeRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<Recipe> SingleWithIncludes()
        {
            return base.SingleWithIncludes()
                .Include(e => e.RecipeSource);
        }

        protected override IQueryable<Recipe> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<List<Recipe>> GetByUserAsync(long userId)
        {
            return ManyWithIncludes()
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public Task<Recipe?> GetByUserAndSourceUrlAsync(long userId, string sourceRecipeUrl)
        {
            return ManyWithIncludes()
                .SingleOrDefaultAsync(e => e.UserId == userId && e.SourceRecipeUrl == sourceRecipeUrl);
        }

        public Task<Recipe?> GetByIdForUserAsync(long recipeId, long userId)
        {
            return ManyWithIncludes()
                .SingleOrDefaultAsync(e => e.Id == recipeId && e.UserId == userId);
        }
    }
}
