using Edurem.Data;
using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Extensions;
using Docker.DotNet.Models;
using Docker.DotNet;
using System.Diagnostics;
using Edurem.Providers;

namespace Edurem.Services
{
    public class ConsoleDockerService : IDockerService
    {
        IRepositoryFactory RepositoryFactory { get; set; }
        IFileService FileService { get; set; }
        IConfiguration Configuration { get; set; }

        public ConsoleDockerService(
            [FromServices] IRepositoryFactory repositoryFactory,
            [FromServices] IFileService fileService,
            [FromServices] IConfiguration configuration)
        {
            RepositoryFactory = repositoryFactory;
            FileService = fileService;
            Configuration = configuration;
        }

        public void CreateDockerfile(List<string> testFilePaths, List<string> codeFilePaths, Language language, string saveToPath)
        {
            ILanguageTestProvider provider = new PythonTestProvider();

            testFilePaths = testFilePaths.Select(path => $".\\{Path.GetRelativePath(Directory.GetParent(saveToPath).FullName, path)}").ToList();
            codeFilePaths = codeFilePaths.Select(path => $".\\{Path.GetRelativePath(Directory.GetParent(saveToPath).FullName, path)}").ToList();

            provider.CreateDockerfile(testFilePaths, codeFilePaths, saveToPath);            
        }

        public void CreateImage(string contextPath, ImageBuildParameters parameters)
        {
            var commands = new List<string>
            {
                $"cd \"{contextPath}\"",
                $"docker build -t \"{parameters.Tags[0]}\" -f \"{parameters.Dockerfile}\" ."
            };

            string output = default;
            var process = GetProcess();
            //process.StartInfo.Arguments = $"/C cd \"{contextPath}\" & docker build -t \"{parameters.Tags[0]}\" -f \"{parameters.Dockerfile}\" . & echo HELLO & docker images -a";
            process.Start();

            
            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    foreach (var command in commands)
                    {
                        sw.WriteLine(command);
                    }
                }
            }

            output = process.StandardOutput.ReadToEnd();
            var errors = process.StandardError.ReadToEnd();
        }

        public async Task<string> StartContainer(string containerId, ContainerStartParameters parameters = null)
        {
            var command = $"docker start \"{containerId}\"";

            var process = GetProcess();
            process.Start();

            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(command);
                }
            }

            var output = process.StandardOutput.ReadToEnd();
            await process.WaitForExitAsync();

            var outputs = output.Split($"docker run \"{containerId}\"");
            output = outputs[1].Split(outputs[0].Split("\r\n").Last())[0];

            return output;
        }

        public async Task<string> CreateContainer(CreateContainerParameters parameters)
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            return (await dockerClient.Containers.CreateContainerAsync(parameters)).ID;
        }

        private Process GetProcess()
        {
            Process process = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;

            process.StartInfo = info;

            return process;
        }

        public async Task<string> RunImage(string imageTag)
        {
            var command = $"docker run {imageTag}";

            var process = GetProcess();
            process.Start();

            string output = default;

            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(command);
                }
            }

            output = process.StandardOutput.ReadToEnd();
            var errors = process.StandardError.ReadToEnd();

            var outputs = output.Split(command);
            output = outputs[1].Split(outputs[0].Split("\r\n").Last())[0];

            return output;
        }
    }
}
