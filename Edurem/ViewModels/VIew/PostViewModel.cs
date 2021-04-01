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

        public List<FileModel> Files { get; set; }

        public void FromPostModel(PostModel postModel, List<FileModel> files)
        {
            Title = postModel.Title;
            Body = postModel.PostBody;
            Files = new(files);
        }
    }
}
