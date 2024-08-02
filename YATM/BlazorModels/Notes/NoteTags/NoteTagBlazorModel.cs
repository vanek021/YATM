namespace YATM.BlazorModels.Notes.NoteTags
{
    public class NoteTagBlazorModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }
        public List<NoteBlazorModel> Notes { get; set; } = new();
    }
}
