using Microsoft.Extensions.DependencyInjection;
using YATM.Models.Constants;
using YATM.Models.Entities.Boards;

namespace YATM.Data.Seeds
{
    public static class BoardSeeds
    {
        public static Board MainBoard => new()
        {
            Name = BoardConstants.MainBoard,
            Description = "Автоматически создаваемая доска",
            Columns = new()
            {
                new BoardColumn()
                {
                    Name = "Нужно сделать",
                    Order = 0,
                    Tasks = new()
                    {
                        new()
                        {
                            Name = "Это тестовая задача колонки",
                            Description = "Тестовая задача, созданная автоматически"
                        }
                    }
                },
                new BoardColumn()
                {
                    Name = "В работе",
                    Order = 1,
                    Tasks = new()
                    {
                        new()
                        {
                            Name = "Это тестовая задача колонки",
                            Description = "Тестовая задача, созданная автоматически"
                        }
                    }
                }, 
                new BoardColumn()
                {
                    Name = "Готово",
                    Order = 2,
                    Tasks = new()
                    {
                        new()
                        {
                            Name = "Это тестовая задача колонки",
                            Description = "Тестовая задача, созданная автоматически"
                        }
                    }
                }
            }
        };

        public static void Initialize(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var boards = ctx.Boards.ToList();

            if (!boards.Any())
            {
                var users = ctx.Users.ToList();

                foreach (var user in users)
                {
                    var board = MainBoard;

                    board.BoardUsers.Add(new BoardUsers()
                    {
                        Board = board,
                        User = user,
                        IsOwner = true
                    });

                    ctx.Boards.Add(board);
                }

                ctx.SaveChanges();
            }
        }
    }
}
