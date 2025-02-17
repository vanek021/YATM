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

        public override void Insert(BoardTask entity)
        {
            var boardColumn = TableReadOnly<BoardColumn>().Single(x => x.Id == entity.BoardColumnId);

            var maxTaskNumber = Table()
                .Where(x => x.Column.BoardId == boardColumn.BoardId)
                .OrderBy(x => x.TaskNumber)
                .Max(x => x.TaskNumber);
            
            entity.TaskNumber = maxTaskNumber + 1;
            
            base.Insert(entity);
        }
    }
}
