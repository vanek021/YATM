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
            return base.SingleWithIncludes()
                .Include(n => n.NoteNoteTags)
                .Include(n => n.NoteTags);
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

        public Task<List<Note>> GetAllNotesByUser(User user, List<long>? tagIds = null)
        {
            var query = ManyWithIncludes()
                .Where(n => n.UserId == user.Id);

            if (tagIds != null && tagIds.Any()) 
                query = query.Where(n => n.NoteTags.Any(t => tagIds.Contains(t.Id)));

            return query.OrderByDescending(n => n.IsPinned)
                    .ThenByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}
