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
        public void CreateImage(string contextPath, ImageBuildParameters parameters);

        public Task<string> StartContainer(string cntainerId, ContainerStartParameters parameters = null);

        /// <summary>
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>ID созданного контейнера</returns>
        public Task<string> CreateContainer(CreateContainerParameters parameters);

        public Task<(string Response, string Errors)> RunImage(string imageTag, string name = null);

        public Task<string> GetContainerId(string containerName);

        public Task RemoveContainer(string containerId);
    }
}
