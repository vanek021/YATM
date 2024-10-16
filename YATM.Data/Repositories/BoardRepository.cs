﻿using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Constants;
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
                .Include(b => b.Users)
                .Include(b => b.Columns)
                    .ThenInclude(c => c.Tasks);
        }

        protected override IQueryable<Board> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<Board?> GetBoardByName(string name, long userId)
        {
            return ManyWithIncludes()
                .SingleOrDefaultAsync(b => b.Name == name && b.Users.Any(u => u.Id == userId));
        }

        public Task<List<Board>> GetAllBoards()
        {
            return ManyWithIncludes()
                .ToListAsync();
        }

        public Task<List<Board>> GetAllBoardsWithoutIncludes(long userId)
        {
            return Table()
                .Where(x => x.Users.Any(u => u.Id == userId))
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public Task<Board?> GetById(long id, long userId)
        {
            return ManyWithIncludes()
                .SingleOrDefaultAsync(x => x.Id == id && x.Users.Any(u => u.Id == userId));
        }

        public Task<Board?> GetDefault(long userId)
        {
            return ManyWithIncludes()
                .Where(x => x.Name == BoardConstants.MainBoard && x.Users.Any(u => u.Id == userId))
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
