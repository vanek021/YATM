using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;

namespace YATM.Models.Entities
{
    public class User : BaseUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
