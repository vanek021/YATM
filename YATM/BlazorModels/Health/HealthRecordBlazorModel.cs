namespace YATM.BlazorModels.Health
{
    public class HealthRecordBlazorModel
    {
        public long Id { get; set; }

        public List<TemperatureRecordBlazorModel> TemperatureRecords { get; set; } = new();
    }
}
