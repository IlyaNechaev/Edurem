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
        public void CreateDockerfile(List<string> testFilePaths, List<string> codeFilePaths, string contextPath, string saveToPath);

        public string CreateUnitTests(TestData tests);

        // HasError - имеется ли фатальная ошибка, из-за которой не удалось провести тестирование
        public (string ResultText, bool HasError) ProvideError(string resultText, string errors);

        public string GetFileExtension();
    }
}
