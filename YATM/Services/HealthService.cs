using AutoMapper;
using YATM.BlazorModels.Health;
using YATM.Data;
using YATM.Models.Entities;
using YATM.Models.Entities.Health;

namespace YATM.Services
{
    public class HealthService
    {
        private readonly Database _db;
        private readonly IMapper _mapper;

        public HealthService(Database db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<HealthRecordBlazorModel> GetOrCreateHealthRecordForAsync(User user, DateOnly date)
        {
            var fromDb = await _db.HealthRecords.GetForAsync(user, date);

            if (fromDb is not null)
                return _mapper.Map<HealthRecordBlazorModel>(fromDb);

            var newRecord = new HealthRecord();

            newRecord.UserId = user.Id;
            newRecord.RecordedAt = date.ToDateTime(TimeOnly.MinValue);

            _db.HealthRecords.Insert(newRecord);
            await _db.SaveChangesAsync();

            return _mapper.Map<HealthRecordBlazorModel>(newRecord);
        }

        public async Task SaveTemperatureRecordForAsync(User user, long healthRecordId, TemperatureRecordBlazorModel temperatureRecordModel)
        {
            var dbHealthRecord = _db.HealthRecords.GetByIdAsync(user, healthRecordId);

            if (dbHealthRecord is null)
                return;

            var tempRecord = _mapper.Map<TemperatureRecord>(temperatureRecordModel);

            tempRecord.HealthRecordId = healthRecordId;

            _db.TemperatureRecords.Insert(tempRecord);
            await _db.SaveChangesAsync();
        }
    }
}
