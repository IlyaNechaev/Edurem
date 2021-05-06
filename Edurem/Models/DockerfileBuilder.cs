using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Edurem.Models
{
    public class DockerfileBuilder
    {
        private StringBuilder content;
        private string lastCommand;
        public DockerfileBuilder()
        {
            content = new();
            lastCommand = string.Empty;
        }

        public void Create(string dockerfilePath)
        {
            dockerfilePath = dockerfilePath.Split("\\").Last() == "Dockerfile" 
                ? dockerfilePath 
                : Path.Combine(dockerfilePath, "Dockerfile");

            using (var file = File.CreateText(dockerfilePath))
            {
                file.Write(content);
            }
        }

        public DockerfileBuilder From(string baseImage)
        {
            if (lastCommand != "FROM" && lastCommand != string.Empty)
                content.AppendLine();

            lastCommand = "FROM";
            content.AppendLine($"FROM {baseImage}");

            return this;
        }

        public DockerfileBuilder Copy(string copyFrom, string copyTo)
        {
            if (lastCommand != "COPY")
                content.AppendLine();

            lastCommand = "COPY";
            content.AppendLine($"COPY \"{copyFrom}\" \"{copyTo}\"");

            return this;
        }

        public DockerfileBuilder Add(string addFrom, string addTo)
        {
            if (lastCommand != "ADD")
                content.AppendLine();

            lastCommand = "ADD";
            content.AppendLine($"ADD \"{addFrom}\" \"{addTo}\"");

            return this;
        }

        public DockerfileBuilder Run(string command, params string[] args)
        {
            if (lastCommand != "RUN")
                content.AppendLine();

            lastCommand = "RUN";
            content.AppendLine($"RUN [ \"{command}\", {string.Join(", ", args.Select(com => $"\"{com}\""))} ]");

            return this;
        }

        public DockerfileBuilder Cmd(string command, params string[] args)
        {
            if (lastCommand != "CMD")
                content.AppendLine();

            lastCommand = "CMD";
            content.AppendLine($"CMD [ \"{command}\", {string.Join(", ", args.Select(com => $"\"{com}\""))} ]");

            return this;
        }
    }
}
