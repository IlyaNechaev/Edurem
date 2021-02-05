using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("notification_options")]
    public class NotificationOptions
    {
        [Key]
        public int Id { get; set; }

        public bool NewTasksToEmail { get; set; }

        public bool TeacherMessageToEmail { get; set; }

        public bool TaskResultToEmail { get; set; }
    }
}
