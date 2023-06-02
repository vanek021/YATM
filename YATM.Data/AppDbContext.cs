using Microsoft.EntityFrameworkCore;
using YATM.Core.Data;
using YATM.Core.Models;
using YATM.Models.Entities;

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
    }
}