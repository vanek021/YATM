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
        public Database(DbContext context, BoardRepository boardRepo, BoardTaskRepository boardTaskRepo, NoteRepository noteRepo, NoteTagRepository noteTagRepo) : base(context)
        {
            Boards = boardRepo;
            BoardTasks = boardTaskRepo;
            Notes = noteRepo;
            NoteTags = noteTagRepo;
        }

        public BoardRepository Boards { get; private set; }
        public BoardTaskRepository BoardTasks { get; private set; }
        public NoteRepository Notes { get; private set; }
        public NoteTagRepository NoteTags { get; private set; }
    }
}
