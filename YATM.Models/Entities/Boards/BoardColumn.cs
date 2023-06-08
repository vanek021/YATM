using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;

namespace YATM.Models.Entities.Boards
{
    public class BoardColumn : BaseRecord
    {
        [ForeignKey(nameof(BoardId))]
        public Board Board { get; set; }
        public long BoardId { get; set; }

        public string Name { get; set; }
        public int Order { get; set; }
        public virtual List<BoardTask> Tasks { get; set; } = new();
    }
}
