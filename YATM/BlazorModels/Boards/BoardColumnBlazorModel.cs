namespace YATM.BlazorModels.Boards
{
    public class BoardColumnBlazorModel
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public virtual List<BoardTaskBlazorModel> Tasks { get; set; } = new();
    }
}
