using Edurem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    // Взаимодействие с контекстом баз данных
    public interface IDbService
    {
        // Найти пользователя по его Id
        public User GetUserById(int userId);

        // Найти пользователя по его Логину
        public User GetUserByLogin(string userLogin);

        // Найти пользователя по его Email
        public User GetUserByEmail(string userEmail);

        // Добавить пользователя в Базу данных
        public Task AddUserAsync(User user, List<Role> userRoles = null);

        // Установить пользователю набор ролей
        public Task SetUserRolesAsync(User user, List<Role> userRoles);

        // Удалить пользователя из Базы данных
        public Task RemoveUserAsync(User user);

        // Изменение пользователя. Выполняется по Id
        public Task SetUser(User user);
    }
}
