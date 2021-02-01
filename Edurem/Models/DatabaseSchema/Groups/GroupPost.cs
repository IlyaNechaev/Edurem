using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("posts_groups")]
    public class GroupPost
    {
        public int PostId { get; set; }
        public int GroupId { get; set; }

        [ForeignKey(nameof(PostId))]
        public PostModel Post { get; set; }

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
    }
}
