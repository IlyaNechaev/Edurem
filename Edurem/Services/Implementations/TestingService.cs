using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace Edurem.Services
{
    public class TestingService : ICodeTestingService
    {
        public bool CheckTests()
        {
            throw new NotImplementedException();
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

        public List<(string Parameters, string Result)> GetRandomTests()
        {
            throw new NotImplementedException();
        }
    }
}
