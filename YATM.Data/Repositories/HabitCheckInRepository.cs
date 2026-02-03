using Microsoft.EntityFrameworkCore;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities.Habits;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<HabitCheckIn>))]
    public class HabitCheckInRepository : WriteableRepository<HabitCheckIn>
    {
        public HabitCheckInRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<HabitCheckIn> SingleWithIncludes()
        {
            return base.SingleWithIncludes()
                .Include(c => c.Habit);
        }

        protected override IQueryable<HabitCheckIn> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<HabitCheckIn?> GetByHabitAndDateAsync(long habitId, DateTime checkInDate)
        {
            return Table()
                .SingleOrDefaultAsync(c => c.HabitId == habitId && c.CheckInDate == checkInDate);
        }

        public Task<List<HabitCheckIn>> GetByUserAndDateAsync(long userId, DateTime checkInDate)
        {
            return ManyWithIncludes()
                .Where(c => c.Habit.UserId == userId && c.CheckInDate == checkInDate)
                .ToListAsync();
        }

        public Task<List<HabitCheckIn>> GetByUserAndRangeAsync(long userId, DateTime fromDate, DateTime toDate)
        {
            return ManyWithIncludes()
                .Where(c => c.Habit.UserId == userId && c.CheckInDate >= fromDate && c.CheckInDate <= toDate)
                .ToListAsync();
        }
    }
}
