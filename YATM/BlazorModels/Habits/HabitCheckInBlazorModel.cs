using System;

namespace YATM.BlazorModels.Habits
{
    public class HabitCheckInBlazorModel
    {
        public long Id { get; set; }
        public long HabitId { get; set; }
        public DateTime CheckInDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? FailureReason { get; set; }
    }
}
