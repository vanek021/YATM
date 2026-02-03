using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using YATM.Core.Models;
using YATM.Models.Entities;

namespace YATM.Models.Entities.Habits
{
    public class Habit : BaseRecord
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsArchived { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }

        public virtual List<HabitCheckIn> CheckIns { get; set; } = new();
    }
}
