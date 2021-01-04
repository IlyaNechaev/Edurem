﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class EmailService : IEmailService<MailMessage>
    {
        public event IEmailService<MailMessage>.SendCompletedHandler SendCompleted;

        public MailMessage CreateEmailMessage(string text, string subject, (string Email, string Name) sender, params (string Email, string Name)[] receivers)
        {
            MailMessage emailMessage = new MailMessage(new MailAddress(sender.Email, sender.Name), new MailAddress(receivers[0].Email, receivers[0].Name));

            foreach (var receiver in receivers.Skip(1))
            {
                emailMessage.To.Add(new MailAddress(receiver.Email, receiver.Name));
            }

            emailMessage.Subject = subject;
            emailMessage.Body = text;

            return emailMessage;
        }

        public async Task SendEmailAsync(MailMessage emailMessage, (string Host, int Port, bool UseSsl) smtpServer, (string Username, string Password) authInfo)
        {
            SmtpClient smtpClient = new SmtpClient(smtpServer.Host, smtpServer.Port);

            smtpClient.Credentials = new System.Net.NetworkCredential(authInfo.Username, authInfo.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.SendCompleted += sendCompletedEventHandler;

            await Task.Run(() => smtpClient.SendAsync(emailMessage, null));
        }

        private void sendCompletedEventHandler(object sender, AsyncCompletedEventArgs e)
        {
            var eventArgs = new SendCompletedEventArgs() { Error = e.Error?.Message, IsFailed = (e.Error != null) };

            SendCompleted?.Invoke(this, eventArgs);
        }
    }
}
