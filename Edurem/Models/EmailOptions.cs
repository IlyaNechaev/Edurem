using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public struct EmailOptions
    {
        // Тело сообщения
        public string Text { get; set; }

        // Тема
        public string Subject { get; set; }

        public (string Email, string Name) Sender { get; set; }

        public List<(string Email, string Name)> Receivers { get; set; }

        // Информация о SMTP-сервере
        public  (string Host, int Port, bool UseSsl) SmtpServer { get; set; }

        // Информация для аутентификации почтового аккаунта
        public  (string Username, string Password) AuthInfo { get; set; }
    }
}
