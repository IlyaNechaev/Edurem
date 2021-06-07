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

        public int SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }

        public ICollection<GroupMember> Members { get; set; }

        public ICollection<PostModel> Posts { get; set; }
    }

    public enum RoleInGroup
    {
        ADMIN = 1,
        MEMBER = 2
    }
}
