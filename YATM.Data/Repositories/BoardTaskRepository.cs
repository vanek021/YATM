using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities.Boards;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<BoardTask>))]
    public class BoardTaskRepository : WriteableRepository<BoardTask>
    {
        public BoardTaskRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<BoardTask> SingleWithIncludes()
        {
            return base.SingleWithIncludes();
        }

        protected override IQueryable<BoardTask> ManyWithIncludes()
        {
            return SingleWithIncludes()
                .Include(x => x.User);
        }
    }
}
