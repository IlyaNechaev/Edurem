using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public class TestResults
    {
        public int Tests { get; set; }
        public int Failures { get; set; }
        public int Success { get; set; }
        public string Error { get; set; }

        public float Completion => (((float)Success/Tests) * 100);
    }
}
