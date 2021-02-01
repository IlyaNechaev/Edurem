using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("posts_files")]
    public class PostFile
    {
        public int PostId { get; set; }

        public int FileId { get; set; }

        [ForeignKey(nameof(PostId))]
        public PostModel Post { get; set; }

        [ForeignKey(nameof(FileId))]
        public FileModel File { get; set; }
    }
}
