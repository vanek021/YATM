using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;

namespace YATM.Models.Entities.Notes
{
    public class Note : BaseRecord
    {
        public string Title { get; set; }
        public string Content { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }
    }
}
