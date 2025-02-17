using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;

namespace YATM.Models.Entities.Boards
{
    public class BoardTask : BaseRecord
    {
        [ForeignKey(nameof(BoardColumnId))]
        public BoardColumn Column { get; set; }
        public long BoardColumnId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public long? UserId { get; set; }

        public long TaskNumber { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ExpireDate { get; set; }
        public Geometry? MapGeometry { get; set; }
    }
}
