using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class MimeEmailService : IEmailService<MimeMessage>
    {
        public event IEmailService<MimeMessage>.SendCompletedHandler SendCompleted;

        public MimeMessage CreateEmailMessage(string text, string subject, (string Email, string Name) sender, params (string Email, string Name)[] receivers)
        {
            var emailMessage = new MimeMessage();

            // Добавляем отправителя
            emailMessage.From.Add(new MailboxAddress(sender.Name, sender.Email));

            // Создаем список из всех получателей
            foreach (var receiver in receivers)
            {
                emailMessage.To.Add(new MailboxAddress(receiver.Name, receiver.Email));
            }
            
            // Тема сообщения
            emailMessage.Subject = subject;
            // Текст сообщения
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = text
            };

            return emailMessage;
        }

        public async Task SendEmailAsync(MimeMessage emailMessage, (string Host, int Port, bool UseSsl) smtpServer, (string Username, string Password) authInfo)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer.Host, smtpServer.Port, smtpServer.UseSsl);
                    await client.AuthenticateAsync(authInfo.Username, authInfo.Password);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }

                var eventArgs = new SendCompletedEventArgs() { IsFailed = false};

                SendCompleted?.Invoke(this, eventArgs);
            }
            catch (Exception ex)
            {
                var eventArgs = new SendCompletedEventArgs() { Error = ex.Message, IsFailed = true };

                SendCompleted?.Invoke(this, eventArgs);
            }
        }
    }
}
