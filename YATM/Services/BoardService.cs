using AutoMapper;
using YATM.BlazorModels.Boards;
using YATM.Data;

namespace YATM.Services
{
    public class BoardService
    {
        private readonly Database _db;
        private readonly IMapper _mapper;

        public BoardService(Database db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<BoardBlazorModel> GetBoardBlazorModelByNameAsync(string boardName)
        {
            var board = await _db.Boards.GetBoardByName(boardName);

            if (board == null)
                return new();

            var boardBlazorModel = _mapper.Map<BoardBlazorModel>(board);

            return boardBlazorModel;
        }

        public async Task<BoardBlazorModel?> GetBoardBlazorModelByIdAsync(long id)
        {
            var board = await _db.Boards.GetByIdAsync(id);

            if (board == null)
                return new();

            var boardBlazorModel = _mapper.Map<BoardBlazorModel>(board);

            return boardBlazorModel;
        }

        public async Task<List<BoardBlazorModel>> GetBoardBlazorModelsAsync()
        {
            var boards = await _db.Boards.GetAllBoards();

            return _mapper.Map<List<BoardBlazorModel>>(boards);
        }

        public async Task DeleteTaskAsync(long taskId)
        {
            var task = await _db.BoardTasks.GetByIdAsync(taskId);

            if (task is null)
                return;

            _db.BoardTasks.Delete(task);
            await _db.SaveChangesAsync();
        }
    }
}
