using Edurem.Data;
using Edurem.Extensions;
using Edurem.Models;
using Microsoft.AspNetCore.Hosting;
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
        IGroupService GroupService { get; init; }
        IWebHostEnvironment AppEnvironment { get; init; }

        public PostService(
            IRepositoryFactory repositoryFactory,
            IFileService fileService,
            ICodeTestService testService,
            IGroupService groupService,
            IConfiguration configuration,
            IWebHostEnvironment appEnvironment)
        {
            RepositoryFactory = repositoryFactory;
            Configuration = configuration;
            FileService = fileService;
            TestService = testService;
            GroupService = groupService;
            AppEnvironment = appEnvironment;
        }

        public async Task AddFilesToPost(int postId, List<(Stream Stream, string Name)> files)
        {
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();
            var post = await PostRepository.Get(post => post.Id == postId, nameof(PostModel.AttachedFiles));

            foreach (var file in files)
            {
                var filePath = Configuration.GetDirectoryPath().ForPostFiles(postId.ToString());

                try
                {
                    // Загрузка файлов на сервер
                    var fileModel = await FileService.UploadFile(file.Stream, filePath, $"{file.Name}");

                    post.AttachedFiles.Add(fileModel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            await PostRepository.Update(post);
        }

        public async Task CreatePost(PostModel post, IEnumerable<int> groupIds, IEnumerable<FileModel> files = null)
        {
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();
            var GroupRepository = RepositoryFactory.GetRepository<Group>();

            try
            {
                post.AttachedFiles = (ICollection<FileModel>)files;

                // Добавляем новую публикацию в БД
                await PostRepository.Add(post);
            }
            catch (Exception)
            {
                throw;
            }


            foreach (var groupId in groupIds)
            {
                var group = await GroupRepository.Get(group => group.Id == groupId);
                if (group.Posts is null) group.Posts = new List<PostModel>();
                group.Posts.Add(post);

                await GroupRepository.Update(group);
            }
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
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();

            var post = await PostRepository.Get(post => post.Id == postId, nameof(PostModel.AttachedFiles));

            for (; post.AttachedFiles.Count > 0;)
            {
                var fileId = post.AttachedFiles.First().Id;

                await FileService.RemoveFile(fileId, true);
            }

            var postDirectory = FileService.GetFullPath(Path.Combine("file_system", "posts", $"post_{post.Id}"));
            if (Directory.Exists(postDirectory))
            {
               Directory.Delete(postDirectory, true);
            }

            await PostRepository.Delete(post);
        }

        public async Task<PostModel> GetPost(int postId)
        {
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();

            var post = await PostRepository.Find(p => p.Id == postId);

            return post.FirstOrDefault();
        }

        public ICollection<string> GetPostFilesPaths(int postId)
        {
            var directoryPath = Configuration.GetDirectoryPath().ForPostFiles(postId.ToString());

            directoryPath = Path.Combine(AppEnvironment.WebRootPath, directoryPath);

            return Directory.Exists(directoryPath) ? Directory.GetFiles(directoryPath).ToList() : new List<string>();
        }

        public async Task<(IEnumerable<string> CommonTestsPaths, IEnumerable<(int UserId, IEnumerable<string> FilePaths)> OptionTestsPaths)> GetPostTestFilesPaths(int postId)
        {
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();

            var groupIds = (await PostRepository.Get(post => post.Id == postId, nameof(PostModel.Groups))).Groups.Select(group => group.Id);

            var userIds = new List<int>();

            foreach (var groupId in groupIds)
            {
                var members = await GroupService.GetMembers(groupId);
                userIds.AddRange(members.Select(gm => gm.UserId));
            }

            var optionTestsDirectoryPath = new List<(int UserId, string DirectoryPath)>();
            var commonTestsDirectoryPath = string.Empty;

            optionTestsDirectoryPath = userIds
                .Select(id =>
                    (id, Path.Combine(AppEnvironment.WebRootPath, Configuration.GetDirectoryPath().ForPostOptionTests(postId.ToString(), id.ToString())))
                )
                .ToList();
            commonTestsDirectoryPath = Path.Combine(AppEnvironment.WebRootPath, Configuration.GetDirectoryPath().ForPostCommonTests(postId.ToString()));

            return (Directory.Exists(commonTestsDirectoryPath) ? Directory.GetFiles(commonTestsDirectoryPath).ToList() : new string[0],
                optionTestsDirectoryPath.Select(path =>
                    (path.UserId, (IEnumerable<string>)(Directory.Exists(path.DirectoryPath) ? Directory.GetFiles(path.DirectoryPath) : new string[0]))
                ));
        }

        public async Task<IEnumerable<int>> GetPostGroupsIds(int postId)
        {
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();

            return (await PostRepository.Get(post => post.Id == postId, nameof(PostModel.Groups))).Groups.Select(group => group.Id);
        }

        public async Task ModifyPost(int postId, PostModel post)
        {
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();

            var currentPost = (await PostRepository.Find(p => p.Id == postId)).FirstOrDefault();

            currentPost.Title = post.Title;
            currentPost.PostBody = post.PostBody;
            currentPost.AttachedFiles = post.AttachedFiles;
            currentPost.PublicationDate = DateTime.Now;

            await PostRepository.Update(currentPost);
        }

        public async Task DeletePostFiles(int postId)
        {
            var postFilesIds = new List<int>();
            var postTestFilesPaths = new List<string>();

            postFilesIds.AddRange(await GetAttachedFilesIds(postId));

            var paths = await GetPostTestFilesPaths(postId);
            postTestFilesPaths.AddRange(paths.CommonTestsPaths);
            postTestFilesPaths.AddRange(paths.OptionTestsPaths.SelectMany(test => test.FilePaths));

            try
            {
                postFilesIds.ForEach(fileId => FileService.RemoveFile(fileId, true));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            postTestFilesPaths.ForEach(path =>
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                };
            });
        }

        public async Task<IEnumerable<int>> GetAttachedFilesIds(int postId)
        {
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();

            return (await PostRepository.Find(p => p.Id == postId, nameof(PostModel.AttachedFiles)))
                .FirstOrDefault()?
                .AttachedFiles
                .Select(af => af.Id)
                .ToList();
        }

        public async Task<IEnumerable<TestInfo>> GetTestResults(int postId)
        {
            var TestInfoRepository = RepositoryFactory.GetRepository<TestInfo>();

            var testsInfo = (await TestInfoRepository.Find(testInfo => testInfo.PostId == postId, nameof(TestInfo.User))).ToList();

            return testsInfo;
        }
    }
}
