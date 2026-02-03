using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YATM.Models.Entities.Health
{
    public class HealthSvgData
    {
        public HashSet<string> SelectedDataPositions { get; set; } = new HashSet<string>();
    }
}
