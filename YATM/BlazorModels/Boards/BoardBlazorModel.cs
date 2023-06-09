namespace YATM.BlazorModels.Boards
{
    public class BoardBlazorModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<BoardColumnBlazorModel> Columns { get; set; } = new();
    }
}
