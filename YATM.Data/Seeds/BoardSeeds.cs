using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Models.Entities.Boards;

namespace YATM.Data.Seeds
{
    public static class BoardSeeds
    {
        private static readonly Board MainBoard = new()
        {
            Name = "Основная доска",
            Description = "Автоматически создаваемая доска",
            Columns = new()
            {
                new BoardColumn()
                {
                    Name = "Нужно сделать",
                    Order = 0
                },
                new BoardColumn()
                {
                    Name = "В работе",
                    Order = 1
                }, 
                new BoardColumn()
                {
                    Name = "Готово",
                    Order = 2
                }
            }
        };

        public static void Initialize(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var board = ctx.Boards.SingleOrDefault(b => b.Name == MainBoard.Name);

            if (board is null)
                ctx.Boards.Add(MainBoard);
                
            ctx.SaveChanges();
        }
    }
}
