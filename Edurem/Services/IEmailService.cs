using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.Services
{
    // Работа с электронной почтой
    public interface IEmailService
    {
        public delegate void SendCompletedHandler(object sender, SendCompletedEventArgs e);

        public Task SendEmailAsync(EmailOptions options);

        public event SendCompletedHandler SendCompleted;
    }

    public class SendCompletedEventArgs : EventArgs
    {
        public string Error { get; init; }

        public bool IsFailed { get; init; }
    }
}
