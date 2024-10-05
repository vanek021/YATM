using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;
using YATM.Models.Entities.Notes;

namespace YATM.Models.Entities.Health
{
    public class HealthRecord : BaseRecord
    {
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }
        public DateTime RecordedAt { get; set; }
        public string? BodyNote { get; set; }
        public string? TemperatureGeneralNote { get; set; }
        [Column(TypeName = "jsonb")]
        public HealthSvgData? HealthSvgData { get; set; }
        public virtual List<TemperatureRecord> TemperatureRecords { get; set; } = new();
    }
}
