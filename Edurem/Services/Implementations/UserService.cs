using Edurem.Data;
using Edurem.Models;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Edurem.Services
{
    public class UserService : IUserService
    {
        IDbService DbService;
        ISecurityService SecurityService;
        User AuthenticatedUser;
        public UserService([FromServices] IDbService dbService,
                           [FromServices] ISecurityService securityService)
        {
            DbService = dbService;
            SecurityService = securityService;
        }

        public async Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> RegisterUser(RegisterViewModel registerModel)
        {
            return await RegisterUser(registerModel, SecurityService);
        }

        public async Task<(bool HasErrors, List<(string Key, string Message)> ErrorMessages)> RegisterUser(RegisterViewModel registerModel, 
                                                                                                           ISecurityService securityService)
        {
            (bool HasErrors, List<(string Key, string Message)> ErrorMessages) result = new();
            result.ErrorMessages = new List<(string Key, string Message)>();

            // Существует ли пользователь с таким логином
            if (DbService.GetUserByLogin(registerModel.Login) != null)
            {
                result.ErrorMessages.Add(("Login", "Логин уже используется"));
            }
            else if (DbService.GetUserByEmail(registerModel.Email) != null)
            {
                result.ErrorMessages.Add(("Email", "Email уже используется"));
            }
            else
            {
                User user = registerModel.ToUser(securityService);
                await DbService.AddUserAsync(user);
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

            var validUser = DbService.GetUserByLogin(userLogin);

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
                AuthenticatedUser = validUser; // Запоминаем пользователя
            }

            result.HasErrors = result.ErrorMessages.Count > 0;

            return result;

            // ЛОКАЛЬНАЯ ФУНКЦИЯ
            // Аутентификация пользователя
            async Task AuthenticateUser()
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, validUser.Login)
                };

                // Добавляем роли пользователя в ClaimIdentity
                foreach (var userRole in validUser.Roles)
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole.Role.Name));
                }
                // Добавление дополнительных клеймов
                claims.Add(new Claim("Status", "REGISTERED"));

                // Создаем объект ClaimsIdentity
                var claimId = new ClaimsIdentity(claims, "EduremCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                // Установка аутентификационных куки
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimId));
            }
        }

        public async Task LogoutUser(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticatedUser = null;
        }

        public User GetAuthenticatedUser(HttpContext context)
        {
            // Если пользователь не авторизован
            if (!context.User.Identity.IsAuthenticated)
                return null;

            return AuthenticatedUser ??= DbService.GetUserByLogin(context.User.Identity.Name);
        }

        public async Task ChangeUser(User user)
        {
            try
            {
                await DbService.SetUser(user);
            }
            catch(DatabaseServiceException)
            {
                throw;
            }
        }
    }
}
