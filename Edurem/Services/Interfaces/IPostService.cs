using Edurem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface IPostService
    {
        public Task AddFilesToPost(int postId, List<FileModel> files);

        public Task AddTestsToPost(int postId, List<FileModel> testFiles);
    }
}