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
        public Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> RegisterUser(RegisterViewModel registerModel);
        public Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> RegisterUser(RegisterViewModel registerModel, ISecurityService securityService);

        // Вход в систему пользователя по логину и паролю
        public Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> SignInUser(string userLogin, string userPassword, HttpContext context);
        public Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> SignInUser(string userLogin, string userPassword, HttpContext context, ISecurityService securityService);

        public Task LogoutUser(HttpContext context);

        // Возвращает авторизованного пользователя
        public User GetAuthenticatedUser(HttpContext context);

        public Task UpdateUser(User user);

        // Возвращает настройки уведомлений для данного пользователя
        public NotificationOptions GetUserNotificationOptions(User user);

        // Отправить на почту пользователя письмо с подтверждением
        public Task SendUserEmailConfirmation(User user, params SendCompletedHandler[] onSendCompleted);
    }
}
