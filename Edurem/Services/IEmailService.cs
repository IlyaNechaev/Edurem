using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    // Работа с электронной почтой
    public interface IEmailService<MessageType>
    {
        public delegate void SendCompletedHandler(object sender, SendCompletedEventArgs e);

        public MessageType CreateEmailMessage(string text, string subject, (string Email, string Name) sender, params (string Email, string Name)[] receivers);

        public Task SendEmailAsync(MessageType emailMessage, (string Host, int Port, bool UseSsl) smtpServer, (string Username, string Password) authInfo);

        public event SendCompletedHandler SendCompleted;
    }

    public class SendCompletedEventArgs : EventArgs
    {
        public string Error { get; init; }

        public bool IsFailed { get; init; }
    }
}
