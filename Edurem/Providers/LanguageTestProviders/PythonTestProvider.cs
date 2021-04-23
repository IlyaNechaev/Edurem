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

        public Language Language { get; init; } = Language.PYTHON;

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

        public string CreateUnitTests(UnitTestInfo tests)
        {
            string testFile = @"
import unittest

class MyTest(unittest.TestCase):

    currentResult = None # holds last result object passed to run method

    @classmethod
    def setResult(cls, amount, errors, failures, skipped):
        cls.amount, cls.errors, cls.failures, cls.skipped = \
            amount, errors, failures, skipped

    def tearDown(self):
        amount = self.currentResult.testsRun
        errors = self.currentResult.errors
        failures = self.currentResult.failures
        skipped = self.currentResult.skipped
        self.setResult(amount, errors, failures, skipped)

    @classmethod
    def tearDownClass(cls):
        print(""\n{"")
        print(""  \""tests_run\"": "" + str(cls.amount) + "","")
        print(""  \""errors\"": "" + str(len(cls.errors)) + "","")
        print(""  \""failures\"": "" + str(len(cls.failures)) + "","")
        print(""  \""success\"": "" + str(cls.amount - len(cls.errors) - len(cls.failures)) + "","")
        print(""}"")

    def run(self, result= None):
        self.currentResult = result # remember result for use in tearDown
        unittest.TestCase.run(self, result) # call superclass run method

    def test(self):
        @test_asserts

if __name__ == '__main__':
    unittest.main()";

            for(int i = 0; i < tests.Parameters.Count; i++)
            {
                var testAssert = $"def test{ i + 1}(self):\n        self.assertEqual({tests.MethodName.Split("").Last()}({string.Join(", ", tests.Parameters[i].Input)}, {tests.Parameters[i].Output}))";
                testFile = testFile.Replace("@test_assert", $"{testAssert}\n\n    @test_assert");
            }

            testFile = testFile.Replace("@test_assert", "");

            return testFile;
        }

        public string GetFileExtension()
        {
            return "py";
        }
    }
}
