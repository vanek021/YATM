using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Infrastructure.Extensions;
using YATM.Models.Entities;
using YATM.Models.Entities.Health;
using YATM.Models.Entities.Notes;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<HealthRecord>))]
    public class HealthRecordRepository : WriteableRepository<HealthRecord>
    {
        public HealthRecordRepository(DbContext context) : base(context)
        {

        }
        protected override IQueryable<HealthRecord> SingleWithIncludes()
        {
            return base.SingleWithIncludes();
        }

        protected override IQueryable<HealthRecord> ManyWithIncludes()
        {
            return SingleWithIncludes()
                .Include(r => r.TemperatureRecords);
        }

        public async Task<HealthRecord?> GetForAsync(User user, DateOnly date)
        {
            var fromDb = await ManyWithIncludes()
                .SingleOrDefaultAsync(r => r.UserId == user.Id && r.RecordedAt.Date == date.ToDateTime(TimeOnly.MinValue).SetUtcDateTimeKind());

            return fromDb;
        }

        public async Task<HealthRecord?> GetByIdAsync(User user, long id)
        {
            return await ManyWithIncludes()
                .SingleOrDefaultAsync(r => r.UserId == user.Id && r.Id == id);
        }
    }
}
