using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public class UnitTestInfo
    {
        public string MethodName { get; set; }
        public List<TestParameter> Parameters { get; set; }
        public UnitTestInfo(List<TestParameter> parameters)
        {
            Parameters = parameters;
        }
        public UnitTestInfo()
        {
            Parameters = new();
        }

        public void Deconstruct(out string methodName, out List<TestParameter> parameters)
        {
            methodName = MethodName;
            parameters = Parameters;
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
}
