using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    // Работа с паролями и шифрованием
    public interface ISecurityService
    {
        // Получить хэш-сумму пароля
        public string GetPasswordHash(string password);

        // Соответствие пароля хэш-сумме
        public bool ValidatePassword(string password, string hash);

        // Генерация случайного пароля длиной length
        public string GeneratePassword(int length = 10);
    }
}
