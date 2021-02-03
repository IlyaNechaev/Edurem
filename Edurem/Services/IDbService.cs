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
        /// <summary>
        /// Изменение записи пользователя, которая имеется в БД. Выполняется по соответствию ID
        /// </summary>
        /// <param name="user">
        /// Измененная запись пользователя
        /// </param>
        public Task UpdateUser(User user);

        // Найти настройки уведомлений для данного пользователя
        public NotificationOptions GetUserNotificationOptions(User user);

        // Возвращает группы, в которых пользователь является участником
        public Task<List<(Group Group, RoleInGroup UserRole)>> GetUserGroups(User user);

        public Task SetEntityProperty<EntityType, ValueType>(EntityType entity, string propertyName, ValueType propertyValue);

        public ValueType GetEntityProperty<EntityType, ValueType>(EntityType entity, string propertyName);

        // Добавляет новую группу в БД
        public Task AddGroupAsync(Group newGroup);
        
        // Добавляет пользователя членом группы
        public Task AddUserToGroupAsync(Group group, User user, RoleInGroup userRole);
    }
}
