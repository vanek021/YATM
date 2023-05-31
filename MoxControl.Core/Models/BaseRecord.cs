﻿using MoxControl.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YATM.Core.Interfaces;
using IDKEY = System.Int64;

namespace YATM.Core.Models
{
    public class BaseRecord : IEntity
    {
        [Key, Unique]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public IDKEY Id { get; set; }

        [Required]
        [Display(Name = "Дата создания")]
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Дата изменения")]
        [ScaffoldColumn(false)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
