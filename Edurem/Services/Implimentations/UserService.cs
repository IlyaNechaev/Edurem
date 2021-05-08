using Edurem.Data;
using Edurem.Models;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Edurem.Extensions;
using static Edurem.Services.IEmailService;
using Microsoft.AspNetCore.Identity;

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
        ICookieService CookieService { get; init; }

        public UserService(IRepositoryFactory repositoryFactory,
                           IDbService dbService,
                           ISecurityService securityService,
                           IEmailService emailService,
                           IFileService fileService,
                           IConfiguration configuration,
                           ICookieService cookieService)
        {
            RepositoryFactory = repositoryFactory;
            DbService = dbService;
            SecurityService = securityService;
            EmailService = emailService;
            FileService = fileService;
            Configuration = configuration;
            CookieService = cookieService;
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
            var NotificationRepository = RepositoryFactory.GetRepository<NotificationOptions>();

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
                var notifications = new NotificationOptions
                {
                    NewTasksToEmail = false,
                    TaskResultToEmail = false,
                    TeacherMessageToEmail = false
                };
                await NotificationRepository.Add(notifications);

                User user = registerModel.ToUser(securityService);
                user.OptionsId = notifications.Id;

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
                    new Claim(ClaimKey.Id, validUser.Id.ToString())
                };

                var cookies = new List<(string Key, string Value)>
                {
                    (ClaimKey.Login, validUser.Login),
                    (ClaimKey.Name, validUser.Name),
                    (ClaimKey.Surname, validUser.Surname),
                    (ClaimKey.Status, "REGISTERED"),
                };

                // Создаем объект ClaimsIdentity
                var claimId = new ClaimsIdentity(claims, "EduremCookie");

                // Установка куки
                context.Response.Cookies.Append(ClaimKey.CookiesId, CookieService.GenerateCookie(cookies));

                // Установка аутентификационных куки
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimId));
            }
        }

        public async Task LogoutUser(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            context.Response.Cookies.Delete(ClaimKey.CookiesId);
        }

        public async Task<User> GetAuthenticatedUser(HttpContext context)
        {
            // Если пользователь не авторизован
            if (!context.User.Identity.IsAuthenticated)
                return null;

            var UserRepository = RepositoryFactory.GetRepository<User>();

            var loginFromCookies = CookieService.GetCookie(ClaimKey.Login, context.Request.GetCookieValue(ClaimKey.CookiesId));

            return await UserRepository.Get(user => user.Login == loginFromCookies);
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                var UserRepository = RepositoryFactory.GetRepository<User>();
                await UserRepository.Update(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            // Каждый раз создается новый код подтверждения почты
            var emailCode = SecurityService.GenerateCode();
            user.EmailCode = emailCode;
            await RepositoryFactory.GetRepository<User>().Update(user);

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

        public async Task UpdateUserNotificationOptions(User user, NotificationOptions options)
        {
            await UpdateUserNotificationOptions(user.Id, options);
        }

        public async Task UpdateUserNotificationOptions(int userId, NotificationOptions options)
        {
            var OptionRepository = RepositoryFactory.GetRepository<NotificationOptions>();

            try
            {
                await OptionRepository.Update(options);
            }
            catch (InvalidOperationException)
            {
                // В данном месте появлялась ошибка, для решения которой приходится доставать объект из БД,
                // присваивать его свойствам необходимые значения, а после обновлять в БД
                var newOptions = await OptionRepository.Get(notificationOptions => notificationOptions.Id == options.Id);

                foreach (var option in newOptions.GetType().GetProperties())
                {
                    option.SetValue(newOptions, options.GetType().GetProperty(option.Name).GetValue(options));
                }

                await OptionRepository.Update(newOptions);
            }
        }

        public async Task<bool> ConfirmEmail(User user, string code)
        {
            var UserRepository = RepositoryFactory.GetRepository<User>();

            var emailCode = user.EmailCode;

            if (emailCode.Equals(code))
            {
                user.Status = Status.ACTIVATED;
                await UserRepository.Update(user);
                return true;
            }

            return false;
        }

        public async Task<bool> IsPasswordValid(int userId, string password)
        {
            var UserRepository = RepositoryFactory.GetRepository<User>();

            var user = await UserRepository.Get(user => user.Id == userId);

            return user.PasswordHash.Equals(SecurityService.GetPasswordHash(password));
        }

        public async Task ChangePassword(int userId, string newPassword)
        {
            var UserRepository = RepositoryFactory.GetRepository<User>();

            var user = await UserRepository.Get(user => user.Id == userId);

            user.PasswordHash = SecurityService.GetPasswordHash(newPassword);

            await UserRepository.Update(user);
        }
    }
}
