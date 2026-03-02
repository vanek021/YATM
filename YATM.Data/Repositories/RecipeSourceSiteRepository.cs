using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities.Recipes;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<RecipeSourceSite>))]
    public class RecipeSourceSiteRepository : WriteableRepository<RecipeSourceSite>
    {
        public RecipeSourceSiteRepository(DbContext context) : base(context)
        {
        }

        protected override IQueryable<RecipeSourceSite> SingleWithIncludes()
        {
            return base.SingleWithIncludes();
        }

        protected override IQueryable<RecipeSourceSite> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<RecipeSourceSite?> GetByCodeAsync(string code)
        {
            return ManyWithIncludes()
                .SingleOrDefaultAsync(s => s.Code == code);
        }
    }
}
