using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using YATM.Core.Attributes;
using YATM.Core.Repositories;

namespace YATM.Data
{
    [CompilerGenerated]
    [Injectable, Injectable(typeof(IDatabase))]
    public class Database : AbstractDatabase
    {
        public Database(DbContext context) : base(context)
        {

        }
    }
}
