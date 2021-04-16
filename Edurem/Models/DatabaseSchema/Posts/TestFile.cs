using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public class TestFile
    {
        public int TestId { get; set; }
        public int FileId { get; set; }

        [ForeignKey(nameof(TestId))]
        public TestInfo Test { get; set; }

        [ForeignKey(nameof(FileId))]
        public FileModel File { get; set; }

    }
}
