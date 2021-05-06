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

        public async Task<(string Response, string Errors)> RunImage(string imageTag, string name = null)
        {
            var command = $"docker run {(name == null ? "" : $"--name {name}")} {imageTag}";

            var process = GetProcess();
            process.Start();

            string output = default;

            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    await sw.WriteLineAsync(command);
                }
            }

            output = process.StandardOutput.ReadToEnd();
            var errors = process.StandardError.ReadToEnd();

            var outputs = output.Split(command);
            output = outputs[1].Split(outputs[0].Split("\r\n").Last())[0];

            return (output, errors);
        }        

        public async Task RemoveContainer(string containerId)
        {
            var commands = new List<string>
            {
                $"docker stop {containerId}",
                $"docker rm {containerId}"
            };
            

            var process = GetProcess();
            process.Start();

            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    foreach (var command in commands)
                    {
                        await sw.WriteLineAsync(command);
                    }
                }
            }

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
        }

        public async Task<string> GetContainerId(string containerName)
        {
            var command = $"(docker ps -qa --filter=\"name=\"{containerName}\"\")";

            var process = GetProcess();
            process.Start();

            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    await sw.WriteLineAsync(command);
                }
            }

            var output = await process.StandardOutput.ReadToEndAsync();
            var errors = process.StandardError.ReadToEndAsync();

            var outputs = output.Split(command);
            output = outputs[1].Split("\r\n")[1].Replace("\n", "");

            return output;
        }
    }
}
