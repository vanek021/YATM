using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Models;

namespace YATM.Models.Entities.Notes
{
    public class NoteTag : BaseRecord
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }

        public virtual List<Note> Notes { get; set; } = new();
        public virtual List<NoteNoteTags> NoteNoteTags { get; set; } = new();
    }
}
