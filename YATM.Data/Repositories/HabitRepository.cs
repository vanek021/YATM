using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities.Habits;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<Habit>))]
    public class HabitRepository : WriteableRepository<Habit>
    {
        public HabitRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<Habit> SingleWithIncludes()
        {
            return base.SingleWithIncludes()
                .Include(h => h.CheckIns);
        }

        protected override IQueryable<Habit> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<List<Habit>> GetAllByUserAsync(long userId, bool includeArchived = false)
        {
            var query = ManyWithIncludes()
                .Where(h => h.UserId == userId);

            if (!includeArchived)
                query = query.Where(h => !h.IsArchived);

            return query.OrderBy(h => h.CreatedAt).ToListAsync();
        }

        public Task<Habit?> GetByIdAsync(long habitId, long userId)
        {
            return ManyWithIncludes()
                .SingleOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);
        }
    }
}
