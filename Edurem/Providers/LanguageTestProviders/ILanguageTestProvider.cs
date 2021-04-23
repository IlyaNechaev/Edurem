using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.Providers
{
    public interface ILanguageTestProvider
    {
        public Language Language { get; init; }

        public void CreateDockerfile(List<string> testFilePaths, List<string> codeFilePaths, string saveToPath);

        public string CreateUnitTests(UnitTestInfo tests);

        public string GetFileExtension();
    }
}
