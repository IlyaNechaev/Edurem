using Edurem.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface IPostService
    {
        public Task AddFilesToPost(int postId, List<(Stream Stream, string Name)> files);

        public Task CreatePost(PostModel post, int groupId, List<FileModel> files = null);
        public Task DeletePost(int postId);

        public Task AddTestsToPost(int postId, List<(Stream Stream, string Name, int UserId)> files);
        public Task AddTestsToPost(int postId, List<(Stream Stream, string Name)> files);
    }
}