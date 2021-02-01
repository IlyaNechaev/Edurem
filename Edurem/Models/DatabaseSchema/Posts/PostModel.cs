using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("posts")]
    public class PostModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public string PostBody { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        public List<PostFile> AttachedFiles { get; set; }
    }
}
