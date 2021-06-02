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
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Edurem.Services
{
    public class TestService : ICodeTestService
    {
        IConfiguration Configuration { get; set; }
        IFileService FileService { get; set; }
        IRepositoryFactory RepositoryFactory { get; set; }
        IDockerService DockerService { get; set; }
        IWebHostEnvironment AppEnvironment { get; set; }
        LanguageTestProviderFactory LanguageTestProviderFactory { get; set; }

        public TestService(IConfiguration configuration,
            IFileService fileService,
            IRepositoryFactory repositoryFactory,
            IDockerService dockerService,
            IWebHostEnvironment appEnvironment,
            LanguageTestProviderFactory languageTestProviderFactory)
        {
            Configuration = configuration;
            FileService = fileService;
            RepositoryFactory = repositoryFactory;
            DockerService = dockerService;
            AppEnvironment = appEnvironment;
            LanguageTestProviderFactory = languageTestProviderFactory;
        }

        public async Task<TestInfo> TestCode(TestInfo testInfo)
        {
            var TestInfoRepository = RepositoryFactory.GetRepository<TestInfo>();
            var imageTag = $"test_{testInfo.PostId}_{testInfo.UserId}";

            var contextPath = FileService.GetFullPath(
                Configuration.GetDirectoryPath().ForContext(
                    testInfo.PostId.ToString()
                    )
                );
            var dockerfilePath = FileService.GetFullPath(
                Configuration.GetDirectoryPath().ForDockerfile(
                    testInfo.PostId.ToString(), testInfo.UserId.ToString()
                    )
                );

            // Создать Docker-образ
            var imageParams = new ImageBuildParameters()
            {
                Dockerfile = $"./{Path.GetRelativePath(contextPath, $"{dockerfilePath}\\Dockerfile")}".Replace("\\", "/"),
                Tags = new List<string>() { imageTag }
            };

            DockerService.CreateImage(contextPath, imageParams);

            string errors = string.Empty;
            string resultText = string.Empty;
            try
            {
                (resultText, errors) = await DockerService.RunImage(imageTag, $"{imageTag}c");
                testInfo.DateOfTesting = DateTime.Now;

                // Обработка ошибок
                var providedErrors = LanguageTestProviderFactory
                    .GetLanguageTestProvider(testInfo.Language)
                    .ProvideError(resultText, errors);

                if (providedErrors.HasError)
                    throw new Exception();

                testInfo.ResultText = providedErrors.ResultText;

                var results = providedErrors.ResultText
                    .Split("%TEST%")
                    .Select(testResults =>
                    {
                        (int Tests, int Success) results = new();
                        try
                        {
                            var testResult = JsonConvert.DeserializeObject<TestResults>(testResults);

                            if (testResult is null) throw new Exception();

                            results = (testResult.Tests, testResult.Success);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                var testResultWithError = JsonConvert.DeserializeObject<TestResults>(testResults.Split("[INFO]")[1]);

                                if (testResultWithError is null) throw new Exception();

                                results = (testResultWithError.Tests, 0);
                            }
                            catch (Exception)
                            {
                                results = (0, 0);
                            }
                        }

                        return results;
                    })
                    .Where(results => results.Tests > 0).ToList();

                testInfo.CountOfTests = results.Sum(result => result.Tests);
                testInfo.CountOfCompletedTests = results.Sum(result => result.Success);

                // Удаление контейнера
                var containerId = await DockerService.GetContainerId($"{imageTag}c");
                await DockerService.RemoveContainer(containerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            await TestInfoRepository.Update(testInfo);

            return testInfo;
        }

        public List<(string TestName, string ResultHtml)> BuildTestResults(TestInfo testInfo)
        {
            var html = testInfo.ResultText;
            var htmlResults = new List<(string TestName, string ResultHtml)>();
            var testResults = html.Split("%TEST%").ToList();

            for (int i = 1; i < testResults.Count; i++)
            {
                // Если пустая строка
                if (string.IsNullOrEmpty(testResults[i].Replace("\n", "").Replace("\r", "").Trim())) continue;
                // Если имеется критическая ошибка
                else if (testResults[i].Contains("[ERROR]"))
                {
                    var error = testResults[i].Split("[ERROR]")[1].Split("[INFO]")[0];
                    html = $"<p class=\"text-danger mb-0\" style=\"font-family: 'Oswald', sans-serif;\">{error}</p>";
                }
                else
                {
                    try
                    {
                        var testResult = JsonConvert.DeserializeObject<TestResults>(testResults[i]);

                        if (!string.IsNullOrEmpty(testResult.Error))
                        {
                            html = $"<p class=\"text-danger mb-0\" style=\"font-family: 'Oswald', sans-serif;\">{testResult.Error}</p>";
                        }
                        else
                        {
                            html = $"<p class=\"mb-0\" style=\"font-family: 'Oswald', sans-serif;\">Тестов выполнено: {testResult.Success}/{testResult.Tests} ({Math.Round(testResult.Completion)}%)</p>";
                        }
                    }
                    catch (JsonReaderException)
                    {
                        html = testResults[i].Replace("\r\n", "\n");
                        html = $"<p class=\"mb-0\" style=\"font-family: 'Oswald', sans-serif;\">{string.Join(@"</p><p class=""mb-0"" style=""font-family: 'Oswald', sans-serif;"">", html.Split("\n").Skip(1))}</p>";
                    }
                    catch (NullReferenceException)
                    {
                        html = string.Empty;
                    }

                }
                htmlResults.Add(($"Тестовый набор {i}", html));
            }

            return htmlResults;
        }

        public async Task<TestInfo> CreateTest(int userId, int postId, Language language)
        {
            // Путь к директориям, в которых хранятся необходимые файлы
            var directoryPaths = new List<string>();

            var TestInfoRepository = RepositoryFactory.GetRepository<TestInfo>();
            var TestFileRepository = RepositoryFactory.GetRepository<TestFile>();

            // Подготовка данных к созданию тестового окружения
            #region PREPARATION

            // Проверка наличия тестовых данных в БД
            var testInfo = await TestInfoRepository.Get(test => test.UserId == userId && test.PostId == postId);
            // Если данных нет, то создается запись
            if (testInfo is null)
            {
                testInfo = new TestInfo
                {
                    UserId = userId,
                    PostId = postId,
                    TestFiles = new()
                };

                await TestInfoRepository.Add(testInfo);
            }
            else
            {
                // Все тестируемые файлы удаляются
                var oldTestFiles = (await TestFileRepository.Find(tf => tf.TestInfoId == testInfo.Id))
                    .ToList();

                await TestFileRepository.DeleteRange(oldTestFiles);
                oldTestFiles.ForEach(tf => FileService.RemoveFile(tf.FileId));
            }
            testInfo.Language = language;

            #endregion

            #region CREATE_JSON_TESTS
            // Создание тестов из JSON-файлов
            // Пути ко всем тестовым JSON-файлам для данного пользователя
            var jsonPaths = new List<string>();

            // Директории, в которых находятся JSON-файлы
            directoryPaths = new()
            {
                FileService.GetFullPath(
                        Configuration.GetDirectoryPath().ForPostOptionTests(postId.ToString(), userId.ToString())
                        ),
                FileService.GetFullPath(
                        Configuration.GetDirectoryPath().ForPostCommonTests(postId.ToString())
                        )
            };

            directoryPaths.ForEach(path => { if (!Directory.Exists(path)) Directory.CreateDirectory(path); });

            // Пути ко всем JSON-файлам
            jsonPaths = directoryPaths
                .SelectMany(path => Directory.GetFiles(path))
                .Where(file => Path.GetExtension(file).Equals(".json"))
                .ToList();

            DeleteUserTestFiles(userId, postId);

            for (int i = 0; i < jsonPaths.Count; i++)
            {
                var jsonPath = jsonPaths[i];

                var jsonParameters = await File.ReadAllTextAsync(jsonPath);

                await CreateUnitTests(postId, userId, jsonParameters, language, $"unit_test_{i + 1}");
            }

            #endregion

            // Создание Dockerfile
            #region CREATE_DOCKERFILE
            // Путь к файлам, содержащим тестовые сценарии
            var testFilePaths = new List<string>();

            directoryPaths = new()
            {
                FileService.GetFullPath(
                        Configuration.GetDirectoryPath().ForPostCommonTests(postId.ToString())
                        ),
                FileService.GetFullPath(
                        Configuration.GetDirectoryPath().ForPostOptionTests(postId.ToString(), userId.ToString())
                        ),
                FileService.GetFullPath(
                        Configuration.GetDirectoryPath().ForPostGeneratedOptionTests(postId.ToString(), userId.ToString())
                        ),
                FileService.GetFullPath(
                        Configuration.GetDirectoryPath().ForPostGeneratedCommonTests(postId.ToString())
                        )
            };

            directoryPaths.ForEach(path => { if (!Directory.Exists(path)) Directory.CreateDirectory(path); });

            testFilePaths = directoryPaths
                .SelectMany(path => Directory.GetFiles(path))
                .Where(file => Path.GetExtension(file).Equals(CodeLanguage.GetLanguageExtension(language)))
                .ToList();

            // Путь к файлам, которые будут протестированы
            var codeFilePaths = Directory.GetFiles(
                FileService.GetFullPath(
                    Configuration.GetDirectoryPath().ForUserSolution(
                        postId.ToString(), userId.ToString()
                        )
                    )
                ).ToList();

            var saveToPath = FileService.GetFullPath(
                Configuration.GetDirectoryPath().ForDockerfile(
                    postId.ToString(), userId.ToString()
                    )
                );

            var contextPath = FileService.GetFullPath(
                Configuration.GetDirectoryPath().ForContext(
                    postId.ToString()
                    )
                );

            LanguageTestProviderFactory
                .GetLanguageTestProvider(language)
                .CreateDockerfile(testFilePaths, codeFilePaths, contextPath, saveToPath);

            #endregion

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

        public async Task CreateUnitTests(int postId, int userId, string jsonParameters, Language language, string unitTestFileName = "unit_test")
        {
            // Десериализация тестовых данных
            object jsonObject;
            try
            {
                jsonObject = DeserializeObjectWithTypes(jsonParameters, typeof(TestData), typeof(List<TestData>), typeof(UserTestData), typeof(List<UserTestData>));
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Переданный JSON имеет неверный формат данных");
            }

            var testInfo = new UserTestInfo();
            testInfo.CreateFrom(jsonObject);

            var languageProvider = LanguageTestProviderFactory.GetLanguageTestProvider(language);

            // Генерация тестов
            var unitTests = new List<string>();
            foreach (var testData in testInfo.Data.Where(data => string.IsNullOrEmpty(data.User) || data.User.Equals(userId.ToString())))
            {
                foreach (var parameter in testData.Tests)
                {
                    unitTests.Add(languageProvider.CreateUnitTests(parameter));
                }
            }

            // Путь к директории, в которой хранятся сгенерированные тесты для пользваотеля userId
            var unitTestsPath = FileService.GetFullPath(
                    Configuration.GetDirectoryPath().ForPostGeneratedOptionTests(
                        postId.ToString(), userId.ToString()
                        )
                    );

            if (!Directory.Exists(unitTestsPath)) Directory.CreateDirectory(unitTestsPath);

            // Сохранение тестовых файлов на сервере
            for (int i = 0; i < unitTests.Count; i++)
            {
                //await File.WriteAllTextAsync(Path.Combine(unitTestsPath, $"{unitTestFileName}_{i + 1}{languageProvider.GetFileExtension()}"), unitTests[i]);
                await File.WriteAllTextAsync(Path.Combine(unitTestsPath, $"{unitTestFileName}{languageProvider.GetFileExtension()}"), unitTests[i]);
            }
        }

        public async Task CreateUnitTests(int postId, string jsonParameters, Language language, string unitTestFileName = "unit_test")
        {
            // Десериализация тестовых данных
            object jsonObject;
            try
            {
                jsonObject = DeserializeObjectWithTypes(jsonParameters, typeof(UserTestData), typeof(List<UserTestData>));
            }
            catch (JsonReaderException)
            {
                throw new InvalidDataException("Переданный JSON имеет неверный формат данных");
            }

            var testInfo = new UserTestInfo();
            testInfo.CreateFrom(jsonObject);

            var languageProvider = LanguageTestProviderFactory.GetLanguageTestProvider(language);

            foreach (var testData in testInfo.Data)
            {
                // Генерация тестов
                var unitTests = new List<string>();
                foreach (var parameters in testData.Tests)
                {
                    unitTests.Add(languageProvider.CreateUnitTests(parameters));
                }

                // Путь к директории, в которой хранятся либо общие тесты, либо по вариантам (Зависит от User)
                var unitTestsPath = string.IsNullOrEmpty(testData.User)
                    ? FileService.GetFullPath(
                        Configuration.GetDirectoryPath().ForPostGeneratedCommonTests(
                            postId.ToString()
                            )
                        )
                    : FileService.GetFullPath(
                        Configuration.GetDirectoryPath().ForPostGeneratedOptionTests(
                            postId.ToString(), testData.User
                            )
                        );

                // Удаление файлов сгенерированных ранее тестов
                Directory.GetFiles(unitTestsPath).ToList().ForEach(file => File.Delete(file));

                // Сохранение тестовых файлов на сервере
                for (int i = 0; i < unitTests.Count; i++)
                {
                    await File.WriteAllTextAsync(Path.Combine(unitTestsPath, $"{unitTestFileName}_{i + 1}{languageProvider.GetFileExtension()}"), unitTests[i]);
                }
            }
        }

        private object DeserializeObjectWithTypes(string json, params Type[] types)
        {
            var methodInfo = typeof(JsonConvert).GetMethod("DeserializeObject", 1, new[] { typeof(string) });

            methodInfo = methodInfo.MakeGenericMethod(types[0]);

            object result;
            try
            {
                result = methodInfo.Invoke(typeof(JsonConvert), new[] { json });
            }
            catch (Exception)
            {
                if (types.Length > 1)
                {
                    try
                    {
                        result = DeserializeObjectWithTypes(json, types.Skip(1).ToArray());
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            return result;
        }

        public async Task UploadTestFile(int postId, int userId, string fileName, Stream stream)
        {
            var filePath = FileService.GetFullPath(Configuration.GetDirectoryPath().ForPostOptionTests(postId.ToString(), userId.ToString()));

            await FileService.UploadFile(stream, filePath, fileName, false);
        }

        public async Task UploadTestFile(int postId, string fileName, Stream stream)
        {
            var filePath = FileService.GetFullPath(Configuration.GetDirectoryPath().ForPostCommonTests(postId.ToString()));

            await FileService.UploadFile(stream, filePath, fileName, false);
        }

        public async Task UploadCodeFiles(int postId, int userId, List<(string Name, Stream Stream)> files)
        {
            var filePath = FileService.GetFullPath(Configuration.GetDirectoryPath().ForUserSolution(postId.ToString(), userId.ToString()));

            // Удаление всех файлов из директории перед загрузкой новых
            try
            {
                Directory.GetFiles(filePath).ToList().ForEach(file => File.Delete(file));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            foreach (var file in files)
            {
                await FileService.UploadFile(file.Stream, filePath, file.Name, false);
            }
        }

        public async Task<List<(Stream Stream, string Name, long Size)>> GetCodeFiles(int postId, int userId)
        {
            List<(Stream Stream, string Name, long Size)> files = new();

            var filesDirectory = FileService.GetFullPath(
                    Configuration.GetDirectoryPath().ForUserSolution(
                        postId.ToString(), userId.ToString()
                        )
                    );

            if (!Directory.Exists(filesDirectory))
                Directory.CreateDirectory(filesDirectory);

            var filePaths = Directory.GetFiles(filesDirectory);

            foreach (var filePath in filePaths)
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    var stream = new MemoryStream((int)fileStream.Length);
                    try
                    {
                        fileStream.CopyTo(stream);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    files.Add((stream, Path.GetFileName(filePath), fileStream.Length));
                }
            }

            return files;
        }

        public bool IsTestCommon(string jsonTest)
        {
            if (JsonConvert.DeserializeObject<UserTestData>(jsonTest) is not null)
            {
                return true;
            }
            else if (JsonConvert.DeserializeObject<List<UserTestData>>(jsonTest) is not null)
            {
                return false;
            }

            throw new InvalidDataException("JSON не соответствует необходимому формату");
        }

        private void DeleteUserTestFiles(int userId, int postId)
        {
            var unitTestsPath = FileService.GetFullPath(
                    Configuration.GetDirectoryPath().ForPostGeneratedOptionTests(
                        postId.ToString(), userId.ToString()
                        )
                    );

            Directory.GetFiles(unitTestsPath).ToList().ForEach(file => File.Delete(file));
        }
    }
}
