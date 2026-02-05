using Microsoft.EntityFrameworkCore;
using YATM.Core.Data;
using YATM.Core.Models;
using YATM.Models.Entities;
using YATM.Models.Entities.Boards;
using YATM.Models.Entities.Habits;
using YATM.Models.Entities.Health;
using YATM.Models.Entities.Notes;
using YATM.Models.Entities.Recipes;

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

            builder.Entity<Board>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Boards)
                .UsingEntity<BoardUsers>();

            builder.Entity<Note>()
                .HasMany(e => e.NoteTags)
                .WithMany(e => e.Notes)
                .UsingEntity<NoteNoteTags>();

            builder.Entity<HabitCheckIn>()
                .HasIndex(e => new { e.HabitId, e.CheckInDate })
                .IsUnique();


            base.OnModelCreating(builder);
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardColumn> BoardColumns { get; set; }
        public DbSet<BoardTask> BoardTasks { get; set; }

        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteTag> NoteTags { get; set; }
        public DbSet<NoteNoteTags> NoteNoteTags { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<TemperatureRecord> TemperatureRecords { get; set; }

        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitCheckIn> HabitCheckIns { get; set; }
    }
}