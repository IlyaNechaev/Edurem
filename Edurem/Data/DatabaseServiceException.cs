using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Data
{
    public class DatabaseServiceException : Exception
    {
        public DatabaseServiceException(string message) : base(message)
        {

        }
    }
}
