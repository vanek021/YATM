using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities;
using YATM.Models.Entities.Notes;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<Note>))]
    public class NoteRepository : WriteableRepository<Note>
    {
        public NoteRepository(DbContext context) : base(context)
        {

        }
        protected override IQueryable<Note> SingleWithIncludes()
        {
            return base.SingleWithIncludes();
        }

        protected override IQueryable<Note> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<List<Note>> GetAllNotes()
        {
            return ManyWithIncludes()
                .ToListAsync();
        }

        public Task<List<Note>> GetAllNotesByUser(User user)
        {
            return ManyWithIncludes()
                .Where(n => n.UserId == user.Id)
                .OrderByDescending(n => n.IsPinned)
                    .ThenBy(n => n.Title)
                .ToListAsync();
        }
    }
}
