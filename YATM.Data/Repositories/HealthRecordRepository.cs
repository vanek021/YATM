using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
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

        public async Task<HealthRecord> GetOrCreateForAsync(User user, DateOnly date)
        {
            var fromDb = await ManyWithIncludes()
                .SingleOrDefaultAsync(r => r.UserId == user.Id && r.RecordedAt.Date == date.ToDateTime(TimeOnly.MinValue));

            if (fromDb is not null)
                return fromDb;

            var newRecord = new HealthRecord();

            newRecord.UserId = user.Id;
            newRecord.RecordedAt = date.ToDateTime(TimeOnly.MinValue);

            Insert(newRecord);
            await _context.SaveChangesAsync();

            return newRecord;
        }

        public async Task<HealthRecord?> GetByIdAsync(User user, long id)
        {
            return await ManyWithIncludes()
                .SingleOrDefaultAsync(r => r.UserId == user.Id && r.Id == id);
        }
    }
}
