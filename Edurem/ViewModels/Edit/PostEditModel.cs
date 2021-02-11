using Edurem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.ViewModels.Edit
{
    public class PostEditModel
    {
        [Required(ErrorMessage = "Введите название для поста")]
        public string Title { get; set; }

        public string PostBody { get; set; }

        public List<FileModel> Files { get; set; }

        public DateTime PublicationDate { get; set; }

        public int AuthorId { get; set; }

        public PostModel ToPostModel()
        {
            var post = new PostModel();

            post.Title = Title;
            post.PostBody = PostBody;
            post.PublicationDate = PublicationDate;
            post.AuthorId = AuthorId;

            return post;
        }
    }
}
