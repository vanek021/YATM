using System.ComponentModel.DataAnnotations;

namespace YATM.BlazorModels.Habits
{
    public class HabitBlazorModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsArchived { get; set; }
    }
}
