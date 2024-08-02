using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities.Notes;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<NoteTag>))]
    public class NoteTagRepository : WriteableRepository<NoteTag>
    {
        public NoteTagRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<NoteTag> SingleWithIncludes()
        {
            return base.SingleWithIncludes()
                .Where(t => !t.IsSoftDeleted)
                .OrderBy(t => t.Order);
        }

        protected override IQueryable<NoteTag> ManyWithIncludes()
        {
            return SingleWithIncludes()
                .Include(t => t.Notes)
                .Include(t => t.NoteNoteTags);
        }

        public Task<List<NoteTag>> GetAllWithNotesAsync()
        {
            return ManyWithIncludes()
                .ToListAsync();
        }
    }
}
