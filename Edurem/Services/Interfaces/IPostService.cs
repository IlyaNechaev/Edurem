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

        public Task CreatePost(PostModel post, IEnumerable<int> groupIds, IEnumerable<FileModel> files = null);
        public Task DeletePost(int postId);

        public Task ModifyPost(int postId, PostModel post);
        public Task AddTestsToPost(int postId, List<(Stream Stream, string Name, int UserId)> files);
        public Task AddTestsToPost(int postId, List<(Stream Stream, string Name)> files);

        public Task<PostModel> GetPost(int postId);
        public Task<IEnumerable<int>> GetPostGroupsIds(int postId);
        public Task<IEnumerable<TestInfo>> GetTestResults(int postId);

        public ICollection<string> GetPostFilesPaths(int postId);
        public Task<(IEnumerable<string> CommonTestsPaths, IEnumerable<(int UserId, IEnumerable<string> FilePaths)> OptionTestsPaths)> GetPostTestFilesPaths(int postId);

        public Task DeletePostFiles(int postId);
        public Task<IEnumerable<int>> GetAttachedFilesIds(int postId);
    }
}