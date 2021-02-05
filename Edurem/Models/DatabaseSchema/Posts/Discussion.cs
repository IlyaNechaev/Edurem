using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("discussions")]
    public class Discussion 
    {
        [Key]
        public int Id { get; set; }

        public int PostId { get; set; }

        public int AuthorId { get; set; }

        public bool IsSolved { get; set; }

        // Тематика обсуждения
        [Required]
        public string Subject { get; set; }

        // Пост, для которого было создано обсуждение
        [ForeignKey(nameof(PostId))]
        public PostModel TargetedPost { get; set; }

        // Автор обсуждения
        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        public List<Message> Messages { get; set; }
    }
}
