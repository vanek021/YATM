using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Models.Entities.Notes;

namespace YATM.Data.Seeds
{
    public static class NoteTagSeeds
    {
        private static List<NoteTag> Tags = new()
        {
            new NoteTag() { Order = 0, Name = "Работа", Color = "#eb8f34", TextColor="#fff" },
            new NoteTag() { Order = 1, Name = "Учеба", Color = "#3492eb", TextColor="#fff" }
        };

        public static void Initialize(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var notesCount = ctx.NoteTags.Count();

            if (notesCount == 0)
                ctx.NoteTags.AddRange(Tags);

            ctx.SaveChanges();
        }
    }
}
