using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("test_files")]
    public class TestFile
    {
        public int TestInfoId { get; set; }
        public int FileId { get; set; }

        [ForeignKey(nameof(TestInfoId))]
        public TestInfo TestInfo { get; set; }

        [ForeignKey(nameof(FileId))]
        public FileModel File { get; set; }
    }
}
