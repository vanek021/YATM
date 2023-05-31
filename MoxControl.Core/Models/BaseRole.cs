using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using YATM.Core.Interfaces;
using IDKEY = System.Int64;

namespace YATM.Core.Models
{
    [Table("Roles")]
    public class BaseRole : IdentityRole<IDKEY>, IEntity
    {
        public BaseRole() { }

        public BaseRole(string roleName)
        {
            Name = roleName;
        }
    }
}
