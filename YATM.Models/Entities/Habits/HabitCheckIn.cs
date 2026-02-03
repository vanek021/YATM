using System;
using System.ComponentModel.DataAnnotations.Schema;
using YATM.Core.Models;

namespace YATM.Models.Entities.Habits
{
    public class HabitCheckIn : BaseRecord
    {
        [ForeignKey(nameof(HabitId))]
        public Habit Habit { get; set; }
        public long HabitId { get; set; }
        public DateTime CheckInDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? FailureReason { get; set; }
    }
}
