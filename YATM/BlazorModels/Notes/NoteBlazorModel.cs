using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YATM.BlazorModels.Notes.NoteTags;

namespace YATM.BlazorModels.Notes
{
    public class NoteBlazorModel
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsPinned { get; set; }

        public List<NoteTagBlazorModel> NoteTags { get; set; } = new();

        [NotMapped]
        public List<long> NoteTagsIds => NoteTags.Select(n => n.Id).ToList();
    }
}
