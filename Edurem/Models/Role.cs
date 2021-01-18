using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public enum Roles
    {
        ADMIN = 1,
        STUDENT,
        TEACHER
    }


    [Table("roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Roles Value { get; set; }

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
