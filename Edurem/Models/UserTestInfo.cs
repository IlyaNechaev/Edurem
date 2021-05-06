using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public class UserTestInfo
    {
        public List<UserTestData> Data { get; private set; }

        public void CreateFrom(object data)
        {
            Data = data switch
            {
                TestData testData => new List<UserTestData>() { new UserTestData { User = null, Tests = new List<TestData> { testData } } },
                List<TestData> testDataList => new List<UserTestData>() { new UserTestData { User = null, Tests = testDataList } },
                UserTestData userTestData => new List<UserTestData>() { userTestData },
                List<UserTestData> userTestDataList => userTestDataList,
                _ => throw new InvalidDataException()
            };
        }
    }

    public class UserTestData
    {
        public string User { get; set; }
        public List<TestData> Tests { get; set; }
    }

    public class TestData
    {
        public string Method { get; set; }
        public List<TestParameter> Runs { get; set; }
        public TestData(List<TestParameter> runs)
        {
            Runs = runs;
        }
        public TestData()
        {
            Runs = new();
        }

        public void Deconstruct(out string method, out List<TestParameter> runs)
        {
            method = Method;
            runs = Runs;
        }
    }

    public class TestParameter
    {
        public TestParameter(List<dynamic> input, dynamic output)
        {
            Input = input;
            Output = output;
        }
        public TestParameter()
        {

        }
        public List<dynamic> Input { get; set; }
        public dynamic Output { get; set; }
    }
}
