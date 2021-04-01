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

namespace Edurem.Services
{
    public class TestingService : ICodeTestingService
    {
        IConfiguration Configuration { get; set; }
        IFileService FileService { get; set; }

        public TestingService([FromServices] IConfiguration configuration,
            [FromServices] IFileService fileService)
        {
            Configuration = configuration;
            FileService = fileService;
        }

        public List<(string Parameters, string Result)> GetPropertyBasedTests()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            dockerClient.Images.BuildImageFromDockerfileAsync(null,
                new ImageBuildParameters
                {
                    Dockerfile = "",
                    Tags = new List<string>() { "" }
                });

            return null;
        }

        public bool TestCode(string testsFilePath, string codeFilePath)
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();



            return false;
        }

        public void CreateDockerfile(List<string> testsFilePath, string codeFilePath, Language language, string saveToPath)
        {
            // Путь к создающемуся Dockerfile
            var dockerfileFullPath = Path.Combine(saveToPath, "Dockerfile");
            // Путь к шаблону Dockerfile для соответствующего языка
            var dockerfilePatternPath = Path.Combine(Configuration.GetFilePath("DockerfilePatterns"), $"{language}.txt");

            using (var file = File.Create(dockerfileFullPath))
            {
                var pattern = FileService.GetFileText(dockerfilePatternPath);

                pattern = pattern.Replace("", "");
            }
        }
    }
}
