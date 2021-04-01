﻿using Edurem.Data;
using Edurem.Models;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Edurem.Extensions;
using Microsoft.AspNetCore.Hosting;
using static Edurem.Services.IEmailService;
using Microsoft.EntityFrameworkCore;

namespace Edurem.Services
{
    public class UserService : IUserService
    {
        ISecurityService SecurityService { get; init; }
        IEmailService EmailService { get; init; }
        IFileService FileService { get; init; }
        IConfiguration Configuration { get; init; }
        IDbService DbService { get; init; }
        IRepositoryFactory RepositoryFactory { get; init; }

        public UserService([FromServices] IRepositoryFactory repositoryFactory,
                           [FromServices] IDbService dbService,
                           [FromServices] ISecurityService securityService,
                           [FromServices] IEmailService emailService,
                           [FromServices] IFileService fileService,
                           [FromServices] IConfiguration configuration)
        {
            RepositoryFactory = repositoryFactory;
            DbService = dbService;
            SecurityService = securityService;
            EmailService = emailService;
            FileService = fileService;
            Configuration = configuration;
        }

        public async Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> RegisterUser(RegisterEditModel registerModel)
        {
            return await RegisterUser(registerModel, SecurityService);
        }

        public async Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> RegisterUser(RegisterEditModel registerModel, 
                                                                                                           ISecurityService securityService)
        {
            (bool HasErrors, List<(string Key, string Message)> ErrorMessages) result = new();
            result.ErrorMessages = new List<(string Key, string Message)>();

            var UserRepository = RepositoryFactory.GetRepository<User>();

            // Существует ли пользователь с таким логином
            if (await UserRepository.Get(user => user.Login == registerModel.Login) != null)
            {
                result.ErrorMessages.Add(("Login", "Логин уже используется"));
            }
            else if (await UserRepository.Get(user => user.Email == registerModel.Email) != null)
            {
                result.ErrorMessages.Add(("Email", "Email уже используется"));
            }
            else
            {
                User user = registerModel.ToUser(securityService);
                await UserRepository.Add(user);
            }
            result.HasErrors = result.ErrorMessages.Count > 0;

            return result;
        }

        public async Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> SignInUser(string userLogin,
                                                                                                         string userPassword,
                                                                                                         HttpContext context)
        {
            return await SignInUser(userLogin, userPassword, context, SecurityService);
        }

        public async Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> SignInUser(string userLogin, 
                                                                                                         string userPassword, 
                                                                                                         HttpContext context, 
                                                                                                         ISecurityService securityService)
        {
            (bool HasErrors, List<(string Key, string Message)> ErrorMessages) result = new();
            result.ErrorMessages = new List<(string Key, string Message)>();

            var UserRepository = RepositoryFactory.GetRepository<User>();

            var validUser = await UserRepository.Get(user => user.Login == userLogin, nameof(User.Roles));

            // Если в базе нет пользователя с такими логином и паролем
            if (validUser == null || !securityService.ValidatePassword(userPassword, validUser?.PasswordHash))
            {
                result.ErrorMessages.Add(("", "Неверно введен логин или пароль"));
            }
            // Если учетная запись пользователя удалена или заблокирована
            else if (!validUser.IsEnabled)
            {
                result.ErrorMessages.Add(("", "Учетная запись недоступна"));
                var errorMessage = validUser.Status switch
                {
                    Status.BLOCKED => "Пользователь временно заблокирован",
                    Status.DELETED => "Учетная запись удалена",
                    _ => "Обратитесь к администратору"
                };
                result.ErrorMessages.Add(("", errorMessage));
            }
            else
            {
                await AuthenticateUser();
            }

            result.HasErrors = result.ErrorMessages.Count > 0;

            return result;

            // ЛОКАЛЬНАЯ ФУНКЦИЯ
            // Аутентификация пользователя
            async Task AuthenticateUser()
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimKey.Login, validUser.Login),
                    new Claim(ClaimKey.Name, validUser.Name),
                    new Claim(ClaimKey.Id, validUser.Id.ToString()),
                    new Claim(ClaimKey.Surname, validUser.Surname),
                    new Claim(ClaimKey.Status, "REGISTERED")
                };


                // Создаем объект ClaimsIdentity
                var claimId = new ClaimsIdentity(claims, "EduremCookie");

                // Установка аутентификационных куки
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimId));
            }
        }

        public async Task LogoutUser(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public User GetAuthenticatedUser(HttpContext context)
        {
            // Если пользователь не авторизован
            if (!context.User.Identity.IsAuthenticated)
                return null;

            var UserRepository = RepositoryFactory.GetRepository<User>();

            return UserRepository.Get(user => user.Login == context.User.FindFirst(ClaimKey.Login).Value).Result;
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                var UserRepository = RepositoryFactory.GetRepository<User>();
                await UserRepository.Update(user);
            }
            catch(DatabaseServiceException)
            {
                throw;
            }
        }

        public async Task<NotificationOptions> GetUserNotificationOptions(User user)
        {
            return await GetUserNotificationOptions(user.Id);
        }

        public async Task<NotificationOptions> GetUserNotificationOptions(int userId)
        {
            var UserRepository = RepositoryFactory.GetRepository<User>();

            return (await UserRepository.Get(u => u.Id == userId, nameof(User.Options))).Options;
        }

        public async Task SendUserEmailConfirmation(User user, params SendCompletedHandler[] onSendCompleted)
        {
            var emailCode = DbService.GetEntityProperty<User, string>(user, "EmailConfirmCode") ?? SecurityService.GeneratePassword();

            // Если свойство emailCode не заполнен (null или пустая строка)
            if (emailCode is null || emailCode.Equals(string.Empty))
            {
                // Генерируем пароль и передаем в БД
                emailCode = SecurityService.GeneratePassword();
                await DbService.SetEntityProperty(user, "EmailConfirmCode", emailCode);
            }

            // Получить текст файла
            var emailMessageText = FileService.GetFileText(Configuration.GetFilePath("ConfirmEmailPattern"));

            // Вставка пароля
            emailMessageText = emailMessageText.Replace("@password", emailCode);

            var emailOptions = new EmailOptions
            {
                Text = emailMessageText,
                Subject = "Подтверждение Email",
                Sender = ("ilia.nechaeff@yandex.ru", "Edurem"),
                Receivers = new() { (user.Email, "") },
                SmtpServer = ("smtp.yandex.ru", 25, false),
                AuthInfo = ("ilia.nechaeff@yandex.ru", "02081956Qw")
            };            

            foreach (var s in onSendCompleted)
            {
                EmailService.SendCompleted += s;
            }

            // Отправить email
            await EmailService.SendEmailAsync(emailOptions);
        }
    }
}
