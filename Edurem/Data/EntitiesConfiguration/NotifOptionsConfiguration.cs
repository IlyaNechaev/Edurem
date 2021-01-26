using Edurem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Data
{
    public class NotifOptionsConfiguration : IEntityTypeConfiguration<NotificationOptions>
    {
        public void Configure(EntityTypeBuilder<NotificationOptions> builder)
        {
            builder.HasKey(options => options.UserId);

            var notifOptions = new NotificationOptions
            {
                NewTasksToEmail = false,
                TaskResultToEmail = false,
                TeacherMessageToEmail = false,
                UserId = 1
            };

            builder
                .HasData(notifOptions);
        }
    }
}
