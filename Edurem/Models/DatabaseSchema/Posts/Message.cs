using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("messages")]
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public int DiscussionId { get; set; }

        public string Text { get; set; }

        [ForeignKey(nameof(DiscussionId))]
        public Discussion TargetedDiscussion { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }
    }
}
