using System.ComponentModel.DataAnnotations;

namespace YATM.BlazorModels.Notes
{
    public class NoteBlazorModel
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
