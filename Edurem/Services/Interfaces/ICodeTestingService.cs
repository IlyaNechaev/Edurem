using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface ICodeTestingService
    {
        public List<(string Parameters, string Result)> GetRandomTests();

        public List<(string Parameters, string Result)> GetPropertyBasedTests();

        public bool CheckTests();
    }
}
