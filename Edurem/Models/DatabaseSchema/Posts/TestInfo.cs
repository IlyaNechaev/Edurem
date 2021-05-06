using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("tests_info")]
    public class TestInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int PostId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(PostId))]
        public PostModel Post { get; set; }

        public string ResultText { get; set; }
        public int CountOfTests { get; set; }
        public int CountOfCompletedTests { get; set; }

        public Language Language { get; set; }

        public List<TestFile> TestFiles { get; set; }
    }
}
