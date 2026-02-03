using YATM.Models.Entities.Health;

namespace YATM.BlazorModels.Health
{
    public class HealthRecordBlazorModel
    {
        public long Id { get; set; }
        public string? BodyNote { get; set; }
        public string? TemperatureGeneralNote { get; set; }
        public HealthSvgData? HealthSvgData { get; set; }
        public List<TemperatureRecordBlazorModel> TemperatureRecords { get; set; } = new();
    }
}
