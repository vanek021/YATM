using AutoMapper;
using YATM.BlazorModels.Boards;
using YATM.Data;
using YATM.Data.Seeds;
using YATM.Models.Entities;
using YATM.Models.Entities.Boards;

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

        public async Task<BoardBlazorModel> CreateBoardAsync(BoardBlazorModel boardBlazorModel, User user)
        {
            var board = _mapper.Map<Board>(boardBlazorModel);

            board.Columns = BoardSeeds.MainBoard.Columns;
            board.BoardUsers.Add(new BoardUsers()
            {
                Board = board,
                User = user,
                IsOwner = true
            });

            _db.Boards.Insert(board);
            await _db.SaveChangesAsync();

            return _mapper.Map<BoardBlazorModel>(board);
        }

        public async Task<BoardBlazorModel> EditBoardAsync(BoardBlazorModel boardBlazorModel, User user)
        {
            var board = await _db.Boards.GetById(boardBlazorModel.Id, user.Id);

            if (board is null)
                throw new ArgumentNullException();

            _mapper.Map(boardBlazorModel, board);

            _db.Boards.Update(board);
            await _db.SaveChangesAsync();

            return _mapper.Map<BoardBlazorModel>(board);
        }

        public async Task<List<BoardShortBlazorModel>> GetBoardBlazorModelsAsync(long userId)
        {
            var boards = await _db.Boards.GetAllBoardsWithoutIncludes(userId);
            return _mapper.Map<List<BoardShortBlazorModel>>(boards);
        }

        public async Task<BoardBlazorModel> GetBoardBlazorModelByIdOrDefaultAsync(long? boardId, long userId)
        {
            Board? board;

            if (boardId.HasValue)
            {
                board = await _db.Boards.GetById(boardId.Value, userId);
            }
            else
            {
                board = await _db.Boards.GetDefault(userId);
            }

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

        public async Task DeleteTaskAsync(long taskId)
        {
            var task = await _db.BoardTasks.GetByIdAsync(taskId);

            if (task is null)
                return;

            _db.BoardTasks.Delete(task);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(BoardTaskBlazorModel taskBlazorModel)
        {
            var task = await _db.BoardTasks.GetByIdAsync(taskBlazorModel.Id);

            if (task is null)
                return;

            _mapper.Map(taskBlazorModel, task);
            _db.BoardTasks.Update(task);
            await _db.SaveChangesAsync();
        }

        public async Task CreateTaskAsync(BoardTaskBlazorModel taskBlazorModel)
        {
            var task = _mapper.Map<BoardTask>(taskBlazorModel);
            _db.BoardTasks.Insert(task);
            await _db.SaveChangesAsync();
        }
    }
}
