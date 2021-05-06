using Edurem.Data;
using Edurem.Extensions;
using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class PostService : IPostService
    {
        IRepositoryFactory RepositoryFactory { get; init; }
        IConfiguration Configuration { get; init; }
        IFileService FileService { get; init; }
        ICodeTestService TestService { get; init; }

        public PostService(
            IRepositoryFactory repositoryFactory,
            IFileService fileService,
            ICodeTestService testService,
            IConfiguration configuration)
        {
            RepositoryFactory = repositoryFactory;
            Configuration = configuration;
            FileService = fileService;
            TestService = testService;
        }

        public async Task AddFilesToPost(int postId, List<(Stream Stream, string Name)> files)
        {
            var PostFileRepository = RepositoryFactory.GetRepository<PostFile>();
            var postFiles = new List<PostFile>();

            foreach (var file in files)
            {
                var filePath = Configuration.GetDirectoryPath().ForPostFiles(postId.ToString());

                try
                {
                    // Загрузка файлов на сервер
                    var fileModel = await FileService.UploadFile(file.Stream, filePath, $"{file.Name}");

                    postFiles.Add(new PostFile { PostId = postId, FileId = fileModel.Id });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            await PostFileRepository.AddRange(postFiles);
        }

        public async Task CreatePost(PostModel post, int groupId, List<FileModel> files = null)
        {
            var PostModelRepository = RepositoryFactory.GetRepository<PostModel>();
            var PostFileRepository = RepositoryFactory.GetRepository<PostFile>();
            var GroupPostRepository = RepositoryFactory.GetRepository<GroupPost>();

            try
            {
                // Добавляем новую публикацию в БД
                await PostModelRepository.Add(post);

                if (files is not null)
                {
                    // Создать связующие записи post_files
                    var postFiles = files.Select(file => new PostFile { FileId = file.Id, PostId = post.Id });

                    // Добавляем связующие записи в БД
                    foreach (var postFile in postFiles)
                    {
                        await PostFileRepository.Add(postFile);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            await GroupPostRepository.Add(new GroupPost { GroupId = groupId, PostId = post.Id });
        }

        public async Task AddTestsToPost(int postId, List<(Stream Stream, string Name, int UserId)> files)
        {
            var PostModelRepository = RepositoryFactory.GetRepository<PostModel>();

            var post = await PostModelRepository.Get(post => post.Id == postId);
            post.HasTests = files is not null && files.Count > 0;

            await PostModelRepository.Update(post);

            foreach (var file in files)
            {
                if (file.UserId == default(int))
                    await TestService.UploadTestFile(postId, file.Name, file.Stream);
                else
                    await TestService.UploadTestFile(postId, file.UserId, file.Name, file.Stream);
            }
        }

        public async Task AddTestsToPost(int postId, List<(Stream Stream, string Name)> files)
        {
            var PostModelRepository = RepositoryFactory.GetRepository<PostModel>();

            var post = await PostModelRepository.Get(post => post.Id == postId);
            post.HasTests = files is not null && files.Count > 0;

            await PostModelRepository.Update(post);

            foreach (var file in files)
            {
                await TestService.UploadTestFile(postId, file.Name, file.Stream);
            }
        }


        public async Task DeletePost(int postId)
        {
            var PostModelRepository = RepositoryFactory.GetRepository<PostModel>();
            var PostFileRepository = RepositoryFactory.GetRepository<PostFile>();

            var post = await PostModelRepository.Get(post => post.Id == postId, nameof(PostModel.AttachedFiles));

            for (; post.AttachedFiles.Count > 0; )
            {
                var postFile = post.AttachedFiles[0];

                var fileId = postFile.FileId;
                await PostFileRepository.Delete(postFile);

                await FileService.RemoveFile(fileId);
            }

            var postDirectory = FileService.GetFullPath(Path.Combine("file_system", "posts", $"post_{post.Id}"));
            if (Directory.Exists(postDirectory))
            {
                Directory.Delete(postDirectory);
            }            

            await PostModelRepository.Delete(post);
        }
    }
}
