using AutoMapper;
using YATM.Models.Entities.Health;

namespace YATM.BlazorModels.Health
{
    public class HealthMapping : Profile
    {
        public HealthMapping()
        {
            CreateMap<HealthRecord, HealthRecordBlazorModel>();
            CreateMap<HealthRecordBlazorModel, HealthRecord>();

            CreateMap<TemperatureRecord, TemperatureRecordBlazorModel>();
            CreateMap<TemperatureRecordBlazorModel, TemperatureRecord>();
        }
    }
}
