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
                .Include(r => r.SourceSite);
        }

        protected override IQueryable<Recipe> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<List<Recipe>> GetAllByUserAsync(long userId)
        {
            return ManyWithIncludes()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public Task<Recipe?> GetByIdAsync(long recipeId, long userId)
        {
            return ManyWithIncludes()
                .SingleOrDefaultAsync(r => r.Id == recipeId && r.UserId == userId);
        }
    }
}
