using System.ComponentModel.DataAnnotations.Schema;
using YATM.Core.Models;
using YATM.Models.Entities;

namespace YATM.Models.Entities.Recipes
{
    public class Recipe : BaseRecord
    {
        public string Title { get; set; }
        public string Content { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }
    }
}
