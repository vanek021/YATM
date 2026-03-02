using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities.Recipes;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<RecipeSource>))]
    public class RecipeSourceRepository : WriteableRepository<RecipeSource>
    {
        public RecipeSourceRepository(DbContext context) : base(context)
        {

        }

        public Task<RecipeSource?> GetByHostAsync(string host)
        {
            return Table()
                .SingleOrDefaultAsync(e => e.Host == host);
        }
    }
}
