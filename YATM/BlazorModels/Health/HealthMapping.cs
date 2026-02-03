using AutoMapper;
using YATM.Infrastructure.Extensions;
using YATM.Models.Entities.Health;

namespace YATM.BlazorModels.Health
{
    public class HealthMapping : Profile
    {
        public HealthMapping()
        {
            CreateMap<HealthRecord, HealthRecordBlazorModel>();
            CreateMap<HealthRecordBlazorModel, HealthRecord>();

            CreateMap<TemperatureRecord, TemperatureRecordBlazorModel>()
                .ForMember(dest => dest.RecordedAt, opt => opt.MapFrom(src => TimeOnly.FromDateTime(src.RecordedAt)))
                .ForMember(dest => dest.RecordedAtDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.RecordedAt)));
            CreateMap<TemperatureRecordBlazorModel, TemperatureRecord>()
                .ForMember(dest => dest.RecordedAt, opt => opt.MapFrom(src => src.RecordedAtDate.ToDateTime(src.RecordedAt).SetUtcDateTimeKind()));
        }
    }
}
