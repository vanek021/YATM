using AutoMapper;
using System.Runtime.CompilerServices;
using YATM.BlazorModels.Health;
using YATM.Data;
using YATM.Infrastructure.Extensions;
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
            newRecord.RecordedAt = date.ToDateTime(TimeOnly.MinValue).SetUtcDateTimeKind();

            _db.HealthRecords.Insert(newRecord);
            await _db.SaveChangesAsync();

            return _mapper.Map<HealthRecordBlazorModel>(newRecord);
        }

        public async Task SaveTemperatureRecordForAsync(User user, long healthRecordId, TemperatureRecordBlazorModel temperatureRecordModel)
        {
            var dbHealthRecord = await _db.HealthRecords.GetByIdAsync(user, healthRecordId);

            if (dbHealthRecord is null)
                return;

            var tempRecord = _mapper.Map<TemperatureRecord>(temperatureRecordModel);

            tempRecord.HealthRecordId = healthRecordId;

            _db.TemperatureRecords.Insert(tempRecord);
            await _db.SaveChangesAsync();
        }

        public async Task SaveTemperatureGeneralNote(User user, HealthRecordBlazorModel healthRecordModel) 
        {
            var dbHealthRecord = await _db.HealthRecords.GetByIdAsync(user, healthRecordModel.Id);

            if (dbHealthRecord is null)
                return;

            dbHealthRecord.TemperatureGeneralNote = healthRecordModel.TemperatureGeneralNote;

            _db.HealthRecords.Update(dbHealthRecord);
            await _db.SaveChangesAsync();
        }

        public async Task SaveHealthBodyNote(User user, HealthRecordBlazorModel healthRecordModel)
        {
            var dbHealthRecord = await _db.HealthRecords.GetByIdAsync(user, healthRecordModel.Id);

            if (dbHealthRecord is null)
                return;

            dbHealthRecord.BodyNote = healthRecordModel.BodyNote;

            _db.HealthRecords.Update(dbHealthRecord);
            await _db.SaveChangesAsync();
        }

        public async Task SaveHealthSvgData(User user, HealthRecordBlazorModel healthRecordModel)
        {
            var dbHealthRecord = await _db.HealthRecords.GetByIdAsync(user, healthRecordModel.Id);

            if (dbHealthRecord is null)
                return;

            dbHealthRecord.HealthSvgData = healthRecordModel.HealthSvgData;

            _db.HealthRecords.Update(dbHealthRecord);
            await _db.SaveChangesAsync();
        }
    }
}
