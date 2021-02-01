using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("groups_members")]
    public class GroupMember
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
    }
}
