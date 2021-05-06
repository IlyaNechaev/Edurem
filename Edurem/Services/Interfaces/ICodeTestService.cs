using Edurem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace Edurem.Services
{
    public interface ICodeTestService
    {
        // Запустить тестирование кода
        public Task<TestInfo> TestCode(TestInfo testInfo);

        // Подготовка данных для тестирования и создание Dockerfile
        public Task<TestInfo> CreateTest(int userId, int postId, Language language);

        public Task SaveTestInfo(TestInfo testInfo);

        public Task<TestInfo> GetTestInfo(int userId, int postId);

        public Task UploadTestFile(int postId, int userId, string fileName, Stream stream);
        public Task UploadTestFile(int postId, string fileName, Stream stream);
        public Task UploadCodeFiles(int postId, int userId, List<(string Name, Stream Stream)> files);
        public Task<List<(Stream Stream, string Name, long Size)>> GetCodeFiles(int postId, int userId);

        public List<(string TestName, string ResultHtml)> BuildTestResults(TestInfo testInfo);
        
        public Task CreateUnitTests(int postId, int userId, string jsonParameters, Language language, string unitTestFileName = "unit_test");
        public Task CreateUnitTests(int postId, string jsonParameters, Language language, string unitTestFileName = "unit_test");
        
        public bool IsTestCommon(string jsonTest);
    }
}
