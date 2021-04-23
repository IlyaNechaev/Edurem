using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;
using Edurem.Extensions;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Edurem.Data;
using Microsoft.AspNetCore.Hosting;
using Edurem.Providers;
using Newtonsoft.Json;

namespace Edurem.Services
{
    public class TestingService : ICodeTestingService
    {
        IConfiguration Configuration { get; set; }
        IFileService FileService { get; set; }
        IRepositoryFactory RepositoryFactory { get; set; }
        IDockerService DockerService { get; set; }
        IWebHostEnvironment AppEnvironment { get; set; }
        IPostService PostService { get; set; }
        LanguageTestProviderFactory LanguageTestProviderFactory { get; set; }

        public TestingService([FromServices] IConfiguration configuration,
            [FromServices] IFileService fileService,
            [FromServices] IRepositoryFactory repositoryFactory,
            [FromServices] IDockerService dockerService,
            [FromServices] IWebHostEnvironment appEnvironment,
            [FromServices] IPostService postService,
            LanguageTestProviderFactory languageTestProviderFactory)
        {
            Configuration = configuration;
            FileService = fileService;
            RepositoryFactory = repositoryFactory;
            DockerService = dockerService;
            AppEnvironment = appEnvironment;
            PostService = postService;
            LanguageTestProviderFactory = languageTestProviderFactory;
        }

        public async Task<TestInfo> TestCode(string dockerfilePath, string contextPath, TestInfo testInfo)
        {
            var TestInfoRepository = RepositoryFactory.GetRepository<TestInfo>();
            var dockerClient = new DockerClientConfiguration().CreateClient();
            var imageTag = $"test_{testInfo.PostId}_{testInfo.UserId}";

            // Создать Docker-образ
            var imageParams = new ImageBuildParameters()
            {
                Dockerfile = $"./{Path.GetRelativePath(contextPath, $"{dockerfilePath}\\Dockerfile")}".Replace("\\", "/"),
                Tags = new List<string>() { imageTag }
            };

            DockerService.CreateImage(contextPath, imageParams);

            try
            {
                testInfo.ResultText = await DockerService.RunImage(imageTag, $"{imageTag}c");
                var containerId = await DockerService.GetContainerId($"{imageTag}c");
                await DockerService.RemoveContainer(containerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                testInfo.ResultText = "Не удалось запустить контейнер";
                return testInfo;
            }

            testInfo.CountOfTests = 5;
            testInfo.CountOfCompletedTests = 0;
            var completion = ((double)testInfo.CountOfCompletedTests / testInfo.CountOfCompletedTests) * 100;

            var status = completion switch
            {
                >= 20 and < 60 => "",
                >= 60 and < 90 => "",
                >= 90 and < 100 => "",
                100 => "",
                _ => "Недостаточное количество выпоненных тестов"
            };

            testInfo.ResultText += $"\nБыли выполнены {testInfo.CountOfCompletedTests}/{testInfo.CountOfTests} тестов. ";

            await TestInfoRepository.Update(testInfo);

            return testInfo;
        }

        public async Task<TestInfo> CreateTest(int userId, int postId, List<int> codeFileIds)
        {
            var TestInfoRepository = RepositoryFactory.GetRepository<TestInfo>();
            var TestFileRepository = RepositoryFactory.GetRepository<TestFile>();
            var PostModelRepository = RepositoryFactory.GetRepository<PostModel>();

            // Проверка наличия тестовых данных в БД
            var testInfo = (await TestInfoRepository.Get(test => test.UserId == userId && test.PostId == postId));
            // Если данных нет, то создается запись
            if (testInfo is null)
            {
                testInfo = new TestInfo
                {
                    UserId = userId,
                    PostId = postId
                };

                await TestInfoRepository.Add(testInfo);
            }
            // Если запись есть, то удаляются файлы с кодом студентов, которые были проетстированы ранее
            else
            {
                await TestFileRepository.DeleteRange(await TestFileRepository.Find(tf => tf.TestId == testInfo.Id));
            }

            // Добавление файлов с кодом студентов
            await TestFileRepository.AddRange(codeFileIds.Select(fileId => new TestFile { FileId = fileId, TestId = testInfo.Id }).ToList());

            testInfo.TestFiles = (await TestFileRepository.Find(tf => tf.TestId == testInfo.Id, nameof(TestFile.File))).ToList();

            // Создание Dockerfile
            var post = await PostModelRepository.Get(post => post.Id == postId);
            // Путь к файлам, содержащим тестовые сценарии
            var testFilePaths = Directory.GetFiles(Path.Combine(AppEnvironment.WebRootPath, post.TestFolderPath)).ToList();
            // Путь к файлам, которые будут протестированы
            var codeFilePaths = (await RepositoryFactory.GetRepository<TestFile>().Find(tf => tf.TestId == testInfo.Id))
                .Select(tf => Path.Combine(AppEnvironment.WebRootPath, tf.File.GetFullPath()))
                .ToList();
            var saveToPath = GetTestsDirectory(userId, postId);

            DockerService.CreateDockerfile(testFilePaths, codeFilePaths, post.Language, saveToPath);

            return testInfo;
        }

        public async Task SaveTestInfo(TestInfo testInfo)
        {
            var TestInfoRepository = RepositoryFactory.GetRepository<TestInfo>();

            await TestInfoRepository.Add(testInfo);
        }

        public async Task<TestInfo> GetTestInfo(int userId, int postId)
        {
            var TestInfoRepository = RepositoryFactory.GetRepository<TestInfo>();

            return (await TestInfoRepository.Find(info => info.UserId == userId && info.PostId == postId)).FirstOrDefault();
        }

        public async Task<FileModel> CreateUnitTests(int postId, string jsonParameters, string unitTestFileName = "unit_test")
        {
            var language = (await RepositoryFactory.GetRepository<PostModel>().Get(post => post.Id == postId)).Language;

            var languageProvider = LanguageTestProviderFactory.GetLanguageTestProvider(language);

            var parameters = JsonConvert.DeserializeObject<UnitTestInfo>(jsonParameters);
            // Task, в котором создаются юнит-тесты
            var unitTests = await Task.Run(() => languageProvider.CreateUnitTests(parameters));

            var unitTestsPath = GetTestsDirectory(postId);

            FileModel unitTestsFile = null;
            // Сохранение тестовых файлов на сервере
            using (var memoryStream = new MemoryStream(unitTests.Length))
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                streamWriter.Write(unitTests);

                unitTestsFile = await FileService.UploadFile(memoryStream, unitTestsPath, $"{unitTestFileName}.{languageProvider.GetFileExtension()}");
            }

            return unitTestsFile;
        }

        public string GetTestsDirectory(int postId, int userId)
        {
            return Path.Combine(AppEnvironment.WebRootPath, $"file_system\\tests\\post_{postId}\\user_{userId}");
        }
        public string GetTestsDirectory(int postId)
        {
            return Path.Combine(AppEnvironment.WebRootPath, $"file_system\\tests\\post_{postId}");
        }

    }
}
