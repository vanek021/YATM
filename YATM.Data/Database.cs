using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Data.Repositories;

namespace YATM.Data
{
    [CompilerGenerated]
    [Injectable, Injectable(typeof(IDatabase))]
    public class Database : AbstractDatabase
    {
        public Database(DbContext context, BoardRepository boardRepo, BoardTaskRepository boardTaskRepo, NoteRepository noteRepo,
            NoteTagRepository noteTagRepo, HealthRecordRepository healthRecordRepo, TemperatureRecordRepository temperatureRecordRepo,
            HabitRepository habitRepo, HabitCheckInRepository habitCheckInRepo, RecipeRepository recipeRepo,
            RecipeSourceRepository recipeSourceRepo) : base(context)
        {
            Boards = boardRepo;
            BoardTasks = boardTaskRepo;
            Notes = noteRepo;
            NoteTags = noteTagRepo;
            HealthRecords = healthRecordRepo;
            TemperatureRecords = temperatureRecordRepo;
            Habits = habitRepo;
            HabitCheckIns = habitCheckInRepo;
            Recipes = recipeRepo;
            RecipeSources = recipeSourceRepo;
        }

        public BoardRepository Boards { get; private set; }
        public BoardTaskRepository BoardTasks { get; private set; }
        public NoteRepository Notes { get; private set; }
        public NoteTagRepository NoteTags { get; private set; }
        public HealthRecordRepository HealthRecords { get; private set; }
        public TemperatureRecordRepository TemperatureRecords { get; private set; }
        public HabitRepository Habits { get; private set; }
        public HabitCheckInRepository HabitCheckIns { get; private set; }
        public RecipeRepository Recipes { get; private set; }
        public RecipeSourceRepository RecipeSources { get; private set; }
    }
}
