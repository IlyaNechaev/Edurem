﻿using Edurem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Providers
{
    public class PythonTestProvider : ILanguageTestProvider
    { 

        public Language Language { get; init; } = Language.PYTHON;

        public void CreateDockerfile(List<string> testFilePaths, List<string> codeFilePaths, string saveToPath)
        {
            var dockerBuilder = new DockerfileBuilder();

            dockerBuilder
                .From("python:3");

            File.WriteAllText(
                Path.Combine(Path.GetDirectoryName(saveToPath), "script.py"),
                CreateScript(testFilePaths.Select(path => Path.GetFileName(path)).ToList())
                );

            testFilePaths.ForEach(file => dockerBuilder.Copy(file.Replace("\\", "/"), "."));
            dockerBuilder.Copy(Path.Combine(saveToPath, "script.py").Replace("\\", "/"), ".");

            codeFilePaths.ForEach(file => dockerBuilder.Copy(file.Replace("\\", "/"), "."));

            dockerBuilder.Cmd("python", "script.py");

            dockerBuilder.Create(saveToPath);

            string CreateScript(List<string> fileNames)
            {
                var script = $"print(\"%TEST%\")\n{string.Join("\nprint(\"%TEST%\")\n", fileNames.Select(name => $"import {name.Split(".")[0]}"))}";

                return script;
            }
        }

        public void CreateDockerfile(List<string> testFilePaths, List<string> codeFilePaths, string contextPath, string saveToPath)
        {
            var dockerBuilder = new DockerfileBuilder();

            // Пути лишь к файлам с кодом
            testFilePaths = testFilePaths.Where(path => Path.GetExtension(path).Equals(GetFileExtension())).ToList();
            codeFilePaths = codeFilePaths.Where(path => Path.GetExtension(path).Equals(GetFileExtension())).ToList();

            dockerBuilder
                .From("python:3");

            File.WriteAllText(
                Path.Combine(saveToPath, "script.py"),
                CreateScript(testFilePaths.Select(path => Path.GetFileName(path)).ToList())
                );

            #region COPY
            testFilePaths.ForEach(file => dockerBuilder.Copy($"./{Path.GetRelativePath(contextPath, file)}".Replace("\\", "/"), "."));
            dockerBuilder.Copy($"./{Path.GetRelativePath(contextPath, Path.Combine(saveToPath, "script.py"))}".Replace("\\", "/"), ".");
            
            codeFilePaths.ForEach(file => dockerBuilder.Copy($"./{Path.GetRelativePath(contextPath, file)}".Replace("\\", "/"), "."));
            #endregion

            #region CMD
            dockerBuilder.Cmd("python", "script.py");
            #endregion

            dockerBuilder.Create(saveToPath);

            string CreateScript(List<string> fileNames)
            {
                var script = $"print(\"%TEST%\")\n{string.Join("\nprint(\"%TEST%\")\n", fileNames.Select(name => $"try:\n    import {name.Split(".")[0]}\nexcept Exception as e:\n    print(\"[ERROR]\" + \"Exception: \" + str(e))"))}";

                return script;
            }
        }

        public string CreateUnitTests(TestData tests)
        {
            string testFile = @"try:
    @import
except Exception as e:
    print(""[ERROR]"" + str(e))
    print(""[INFO]"")

class MyTest():

    currentResult = { ""tests"": 0, ""failures"": 0, ""success"": 0, ""error"": """" } # holds last result object passed to run method

    def printResults(self):
        print(""{"")
        print(""  \""tests\"": "" + str(self.currentResult[""tests""]) + "","")
        print(""  \""failures\"": "" + str(self.currentResult[""failures""]) + "","")
        print(""  \""success\"": "" + str(self.currentResult[""success""]) + "","")
        print(""  \""error\"": \"""" + str(self.currentResult[""error""]) + ""\"""")
        print(""}"")

    def run_tests(self, params):
        for param in params:
            self.currentResult[""tests""] += 1
            try:
                if @method(@input_params) == param[""output""]:
                    self.currentResult[""success""] += 1
                else:
                    self.currentResult[""failures""] += 1
            except Exception as e:
                if self.currentResult[""error""] == """":
                    self.currentResult[""error""] = e

test = MyTest()
params = [ @params ]

test.run_tests(params)
test.printResults()
";

            testFile = testFile
                .Replace("@import", $"from {tests.Method.Split(".")[0]} import {tests.Method.Split(".")[1]} as test_method")
                .Replace("@method", "test_method")
                .Replace("@params", string.Join(", ", tests.Runs.Select(run => $"{{ \"input\": [{string.Join(", ", run.Input)}], \"output\": {run.Output.ToString(new CultureInfo("en-US"))} }}")))
                .Replace("@input_params", string.Join(", ", Enumerable.Range(0, tests.Runs[0].Input.Count).Select(index => $"param[\"input\"][{index}]")));

            return testFile;
        }

        public string GetFileExtension() => ".py";

        public (string ResultText, bool HasError) ProvideError(string resultText, string errors)
        {
            if (errors.Contains("docker:"))
            {
                return (resultText, true);
            }
            else if (errors.Contains("Traceback"))
            {

            }

            return (resultText, false);
        }
    }
}
