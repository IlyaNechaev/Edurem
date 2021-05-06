using Edurem.Models;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Edurem.Services.IEmailService;

namespace Edurem.Services
{
    public interface IUserService
    {
        // Регистрация нового пользователя
        public Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> RegisterUser(RegisterEditModel registerModel);
        public Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> RegisterUser(RegisterEditModel registerModel, ISecurityService securityService);

        // Вход в систему пользователя по логину и паролю
        public Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> SignInUser(string userLogin, string userPassword, HttpContext context);
        public Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> SignInUser(string userLogin, string userPassword, HttpContext context, ISecurityService securityService);

        public Task LogoutUser(HttpContext context);

        // Возвращает авторизованного пользователя
        public Task<User> GetAuthenticatedUser(HttpContext context);

        public Task UpdateUser(User user);

        // Возвращает настройки уведомлений для данного пользователя
        public Task<NotificationOptions> GetUserNotificationOptions(User user);        
        public Task<NotificationOptions> GetUserNotificationOptions(int userId);

        // Обновляет настройки уведомлений для данного пользователя
        public Task UpdateUserNotificationOptions(User user, NotificationOptions options);
        public Task UpdateUserNotificationOptions(int userId, NotificationOptions options);

        // Отправить на почту пользователя письмо с подтверждением
        public Task SendUserEmailConfirmation(User user, params SendCompletedHandler[] onSendCompleted);

        public Task<bool> ConfirmEmail(User user, string password);

        public Task<bool> IsPasswordValid(int userId, string password);

        public Task ChangePassword(int userId, string newPassword);
    }
}
