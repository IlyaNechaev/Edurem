using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("groups")]
    public class Group
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<GroupMember> Members { get; set; }

        public List<GroupPost> GroupPosts { get; set; }
    }

    public enum RoleInGroup
    {
        ADMIN,
        MEMBER
    }
}
