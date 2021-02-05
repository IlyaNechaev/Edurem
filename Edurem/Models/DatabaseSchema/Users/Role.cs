using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Flags]
    public enum Roles
    {
        ADMIN = 1 << 0,
        USER = 1 << 1
    }


    [Table("roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Column(nameof(Name))]
        public string Name { get; private set; }

        [NotMapped]
        public Roles Value 
        { 
            get => (Roles)Enum.Parse(typeof(Roles), Name); 
            set => Name = Enum.GetName(typeof(Roles), value); 
        }

        public List<UserRole> Users { get; set; }

        public Role()
        {
            Id = -1;
            Name = "";

            Users = new List<UserRole>();
        }
        public Role(Roles role)
        {
            Id = (int)role;
            Name = role.ToString();
            Value = role;

            Users = new List<UserRole>();
        }
    }
}
