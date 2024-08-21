using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;

namespace YATM.Models.Entities.Health
{
    public class TemperatureRecord : BaseRecord
    {
        [ForeignKey(nameof(HealthRecordId))]
        public HealthRecord HealthRecord { get; set; }
        public long HealthRecordId { get; set; }

        [Range(30.0, 45.0)]
        public double TempValue { get; set; }
        public string? Note { get; set; }
    }
}
