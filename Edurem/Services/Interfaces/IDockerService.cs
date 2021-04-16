using Docker.DotNet.Models;
using Edurem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface IDockerService
    {
        public void CreateDockerfile(List<string> testFilePaths, List<string> codeFilePaths, Language language, string saveToPath);

        public void CreateImage(string contextPath, ImageBuildParameters parameters);

        public Task<string> StartContainer(string cntainerId, ContainerStartParameters parameters = null);

        /// <summary>
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>ID созданного контейнера</returns>
        public Task<string> CreateContainer(CreateContainerParameters parameters);

        public Task<string> RunImage(string imageTag);
    }
}
