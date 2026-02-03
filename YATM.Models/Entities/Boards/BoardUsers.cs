using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Models.Entities.Notes;

namespace YATM.Models.Entities.Boards
{
    public class BoardUsers
    {
        [ForeignKey(nameof(BoardId))]
        public Board Board { get; set; }
        public long BoardId { get; set; }

        public bool IsOwner { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }
    }
}
