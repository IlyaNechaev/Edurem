using Edurem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.ViewModels
{
    public class PostViewModel
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public IEnumerable<FileModel> Files { get; set; }

        public void FromPostModel(PostModel postModel, IEnumerable<FileModel> files)
        {
            Title = postModel.Title;
            Body = postModel.PostBody;
            Files = files;
        }
    }
}
