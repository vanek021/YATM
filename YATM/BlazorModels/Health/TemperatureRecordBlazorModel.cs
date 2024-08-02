using System.ComponentModel.DataAnnotations;

namespace YATM.BlazorModels.Health
{
    public class TemperatureRecordBlazorModel
    {
        public long Id { get; set; }
        [Range(30.0, 45.0)]
        public double TempValue { get; set; }
        public string Note { get; set; }
    }
}
