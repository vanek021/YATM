namespace YATM.BlazorModels.Boards
{
    public class BoardBlazorModel : BoardShortBlazorModel
    {
        public List<BoardColumnBlazorModel> Columns { get; set; } = new();
    }
}
