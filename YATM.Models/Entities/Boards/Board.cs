using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;
using YATM.Models.Entities.Notes;

namespace YATM.Models.Entities.Boards
{
    public class Board : BaseRecord
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }

        public virtual List<User> Users { get; set; } = new();
        public virtual List<BoardUsers> BoardUsers { get; set; } = new();
        public virtual List<BoardColumn> Columns { get; set; } = new();
    }
}
