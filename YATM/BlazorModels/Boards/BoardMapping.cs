using AutoMapper;
using YATM.Models.Entities.Boards;

namespace YATM.BlazorModels.Boards
{
    public class BoardMapping : Profile
    {
        public BoardMapping()
        {
            CreateMap<Board, BoardBlazorModel>();
            CreateMap<BoardBlazorModel, Board>();

            CreateMap<BoardColumn, BoardColumnBlazorModel>();
            CreateMap<BoardColumnBlazorModel, BoardColumn>();

            CreateMap<BoardTask, BoardTaskBlazorModel>();
            CreateMap<BoardTaskBlazorModel, BoardTask>();
        }
    }
}
