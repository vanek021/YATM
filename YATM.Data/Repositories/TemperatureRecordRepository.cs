using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Models.Entities;
using YATM.Models.Entities.Health;

namespace YATM.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<TemperatureRecord>))]
    public class TemperatureRecordRepository : WriteableRepository<TemperatureRecord>
    {
        public TemperatureRecordRepository(DbContext context) : base(context)
        {

        }
    }
}
