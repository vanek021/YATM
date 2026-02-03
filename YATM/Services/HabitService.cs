using AutoMapper;
using YATM.BlazorModels.Habits;
using YATM.Data;
using YATM.Infrastructure.Extensions;
using YATM.Models.Entities;
using YATM.Models.Entities.Habits;

namespace YATM.Services
{
    public class HabitService
    {
        private readonly Database _db;
        private readonly IMapper _mapper;

        public HabitService(Database db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<HabitBlazorModel>> GetHabitsByUserAsync(User user, bool includeArchived = false)
        {
            var habits = await _db.Habits.GetAllByUserAsync(user.Id, includeArchived);
            return _mapper.Map<List<HabitBlazorModel>>(habits);
        }

        public async Task<HabitBlazorModel> CreateHabitAsync(User user, HabitBlazorModel model)
        {
            var habit = _mapper.Map<Habit>(model);
            habit.UserId = user.Id;
            _db.Habits.Insert(habit);
            await _db.SaveChangesAsync();
            return _mapper.Map<HabitBlazorModel>(habit);
        }

        public async Task UpdateHabitAsync(User user, HabitBlazorModel model)
        {
            var habit = await _db.Habits.GetByIdAsync(model.Id, user.Id);
            if (habit == null)
                return;

            _mapper.Map(model, habit);
            _db.Habits.Update(habit);
            await _db.SaveChangesAsync();
        }

        public async Task ArchiveHabitAsync(User user, long habitId)
        {
            var habit = await _db.Habits.GetByIdAsync(habitId, user.Id);
            if (habit == null)
                return;

            habit.IsArchived = true;
            _db.Habits.Update(habit);
            await _db.SaveChangesAsync();
        }

        public async Task<List<HabitCheckInBlazorModel>> GetCheckInsForDateAsync(User user, DateOnly date)
        {
            var dateUtc = date.ToDateTime(TimeOnly.MinValue).SetUtcDateTimeKind();
            var checkIns = await _db.HabitCheckIns.GetByUserAndDateAsync(user.Id, dateUtc);
            return _mapper.Map<List<HabitCheckInBlazorModel>>(checkIns);
        }

        public async Task<List<HabitCheckInBlazorModel>> GetCheckInsForRangeAsync(User user, DateOnly fromDate, DateOnly toDate)
        {
            var fromUtc = fromDate.ToDateTime(TimeOnly.MinValue).SetUtcDateTimeKind();
            var toUtc = toDate.ToDateTime(TimeOnly.MinValue).SetUtcDateTimeKind();
            var checkIns = await _db.HabitCheckIns.GetByUserAndRangeAsync(user.Id, fromUtc, toUtc);
            return _mapper.Map<List<HabitCheckInBlazorModel>>(checkIns);
        }

        public async Task<HabitCheckInBlazorModel> SaveCheckInAsync(User user, HabitCheckInBlazorModel model)
        {
            var habit = await _db.Habits.GetByIdAsync(model.HabitId, user.Id);
            if (habit == null)
                throw new ArgumentException("Habit not found.");

            var normalizedDate = model.CheckInDate.Date.SetUtcDateTimeKind();
            model.CheckInDate = normalizedDate;

            if (model.IsCompleted)
            {
                model.FailureReason = null;
            }
            else if (string.IsNullOrWhiteSpace(model.FailureReason))
            {
                throw new ArgumentException("Failure reason is required.");
            }

            var existing = await _db.HabitCheckIns.GetByHabitAndDateAsync(model.HabitId, normalizedDate);
            if (existing == null)
            {
                var entity = _mapper.Map<HabitCheckIn>(model);
                _db.HabitCheckIns.Insert(entity);
                await _db.SaveChangesAsync();
                return _mapper.Map<HabitCheckInBlazorModel>(entity);
            }

            _mapper.Map(model, existing);
            _db.HabitCheckIns.Update(existing);
            await _db.SaveChangesAsync();
            return _mapper.Map<HabitCheckInBlazorModel>(existing);
        }

        public async Task<List<HabitFailureReasonStat>> GetFailureReasonStatsAsync(User user, DateOnly fromDate, DateOnly toDate)
        {
            var checkIns = await GetCheckInsForRangeAsync(user, fromDate, toDate);

            return checkIns
                .Where(c => !c.IsCompleted && !string.IsNullOrWhiteSpace(c.FailureReason))
                .Select(c => c.FailureReason!.Trim())
                .Where(reason => reason.Length > 0)
                .GroupBy(reason => reason, StringComparer.OrdinalIgnoreCase)
                .Select(group => new HabitFailureReasonStat
                {
                    Reason = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(stat => stat.Count)
                .ThenBy(stat => stat.Reason)
                .ToList();
        }
    }
}
