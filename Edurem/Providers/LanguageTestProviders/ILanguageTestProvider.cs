using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Providers
{
    public interface ILanguageTestProvider
    {
        public void CreateDockerfile(List<string> testFilePaths, List<string> codeFilePaths, string saveToPath);
    }
}
