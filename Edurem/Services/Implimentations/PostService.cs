using Edurem.Data;
using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class PostService : IPostService
    {
        IRepositoryFactory RepositoryFactory { get; init; }

        public PostService([FromServices] IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }
        
        public async Task AddFilesToPost(int postId, List<FileModel> files)
        {
            var PostFileRepository = RepositoryFactory.GetRepository<PostFile>();
            var postFiles = new List<PostFile>();

            foreach (var file in files)
            {
                postFiles.Add(new PostFile { PostId = postId, FileId = file.Id });
            }

            await PostFileRepository.AddRange(postFiles);
        }

        public async Task AddTestsToPost(int postId, List<FileModel> testFiles)
        {
            var PostModelRepository = RepositoryFactory.GetRepository<PostModel>();

            // Путь к папке с тестами
            var testFolderPath = testFiles.First().GetFullPath();
            // Получить публикацию по id
            var post = await PostModelRepository.Get(post => post.Id == postId);
            // Добавить публикации путь к тестам
            post.TestFolderPath = testFolderPath;

            await PostModelRepository.Update(post);
        }
    }
}
