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
        public string Title { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public string PostBody { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        public List<PostFile> AttachedFiles { get; set; }

        public string GetShortBody(int length = 150)
        {
            // Если длина тела публикации меньше length,
            // то возвращается тело целиком
            if (length >= PostBody.Length)
            {
                return PostBody;
            }
            // Если длина тела публикации больше length,
            // то проводиться сокращение тела
            else
            {
                var bodyParts = PostBody.Substring(0, length).Split("<");
                var shortBodyFirst = string.Join("<", bodyParts.TakeWhile((@string, index) => (index + 1) != bodyParts.Length));
                var shortBodySecond = bodyParts.Last();

                // Если блок последнего элемента не закрыт
                if (!shortBodySecond.Contains(">"))
                {
                    return $"{shortBodyFirst}...";
                }
                else
                {
                    return $"{shortBodyFirst}<{shortBodySecond}...";
                }
            }
        }
    }
}
