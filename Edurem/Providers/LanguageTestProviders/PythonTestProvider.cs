using Edurem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Providers
{
    public class PythonTestProvider : ILanguageTestProvider
    { 

        public Language Language { get; private set; } = Language.PYTHON;

        public void CreateDockerfile(List<string> testFilePaths, List<string> codeFilePaths, string saveToPath)
        {
            var dockerBuilder = new DockerfileBuilder();

            dockerBuilder
                .From("python:3");

            testFilePaths.ForEach(file => dockerBuilder.Copy(file.Replace("\\", "/"), "."));
            codeFilePaths.ForEach(file => dockerBuilder.Copy(file.Replace("\\", "/"), "."));

            testFilePaths.ForEach(file => dockerBuilder.Cmd("python", Path.GetFileName(file)));

            dockerBuilder.Create(saveToPath);
        }
    }
}
