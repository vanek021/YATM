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
        public Database(DbContext context, BoardRepository boardRepo) : base(context)
        {
            Boards = boardRepo;
        }

        public BoardRepository Boards { get; private set; }
    }
}
