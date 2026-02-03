using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;
using YATM.Models.Entities.Boards;
using YATM.Models.Entities.Notes;

namespace YATM.Models.Entities
{
    public class User : BaseUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual List<Board> Boards { get; set; } = new();
    }
}
