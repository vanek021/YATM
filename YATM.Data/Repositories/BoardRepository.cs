using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities.Boards;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<Board>))]
    public class BoardRepository : WriteableRepository<Board>
    {
        public BoardRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<Board> SingleWithIncludes()
        {
            return base.SingleWithIncludes()
                .Include(b => b.Columns)
                    .ThenInclude(c => c.Tasks);
        }

        protected override IQueryable<Board> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<Board?> GetBoardByName(string name)
        {
            return ManyWithIncludes()
                .SingleOrDefaultAsync(b => b.Name == name);
        }

        public Task<List<Board>> GetAllBoards()
        {
            return ManyWithIncludes()
                .ToListAsync();
        }
    }
}
