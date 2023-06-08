using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;

namespace YATM.Models.Entities.Boards
{
    public class Board : BaseRecord
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public virtual List<BoardColumn> Columns { get; set; } = new();
    }
}
