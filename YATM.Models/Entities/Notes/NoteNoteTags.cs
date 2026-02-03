using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YATM.Models.Entities.Notes
{
    public class NoteNoteTags
    {
        [ForeignKey(nameof(NoteId))]
        public Note Note { get; set; }
        public long NoteId { get; set; }

        [ForeignKey(nameof(NoteTagId))]
        public NoteTag NoteTag { get; set; }
        public long NoteTagId { get; set; }
    }
}
