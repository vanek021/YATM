using Microsoft.EntityFrameworkCore;
using YATM.Core.Data;
using YATM.Core.Models;
using YATM.Models.Entities;
using YATM.Models.Entities.Boards;
using YATM.Models.Entities.Notes;

namespace YATM.Data
{
    public class AppDbContext : BaseDbContext<User, BaseRole>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("postgis");

            base.OnModelCreating(builder);
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardColumn> BoardColumns { get; set; }
        public DbSet<BoardTask> BoardTasks { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}