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
    public interface ICodeTestingService
    {
        public Task<TestInfo> TestCode(string dockerfilePath, string contextPath, TestInfo testInfo);

        public Task<TestInfo> CreateTest(int userId, int postId, List<int> codeFileIds);

        public Task SaveTestInfo(TestInfo testInfo);

        public Task<TestInfo> GetTestInfo(int userId, int postId);
    }
}
