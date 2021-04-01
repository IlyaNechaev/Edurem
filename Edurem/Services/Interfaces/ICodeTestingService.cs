using Edurem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface ICodeTestingService
    {
        public bool TestCode(string testsFilePath, string codeFilePath);

        public void CreateDockerfile(List<string> testsFilePath, string codeFilePath, Language language, string saveToPath);
    }
}
