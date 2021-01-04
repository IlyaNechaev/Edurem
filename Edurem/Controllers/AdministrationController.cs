using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Edurem.Models;
using Edurem.Services;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Edurem.Controllers
{
    public class AdministrationController : Controller
    {
        IUserService UserService;
        public AdministrationController([FromServices] IUserService userService)
        {
            UserService = userService;
        }

        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            // Если пользователь авторизован
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Home", "Account");

            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Присутствуют ли в моделе ошибки
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Регистрация пользователя
            var registerResult = await UserService.RegisterUser(model);

            // Если во время регистрации были получены ошибки
            if (registerResult.HasErrors)
            {
                // Добавить все ошибки в ModelState для передач в представление
                registerResult.ErrorMessages.ForEach(error => ModelState.AddModelError(error.Key, error.Message));
                return View(model);
            }

            return Redirect("home");
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            // Если пользователь авторизован
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Home", "Account");

            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Проверяем, присутствуют ли в моделе ошибки
            if (ModelState.IsValid)
            {
                var result = await UserService.SignInUser(model.Login, model.Password, HttpContext);

                // Если во время аутентификации были получены ошибки
                if (result.HasErrors)
                {
                    // Добавить все ошибки в ModelState для передач в представление
                    result.ErrorMessages.ForEach(error => ModelState.AddModelError(error.Key, error.Message));
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Home", "Account");
                }
            }

            return View(model);
        }

        
        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Адрес, с которого был произведен запрос
            var referer = new Uri(Request.Headers["Referer"].Last()).LocalPath;

            await UserService.LogoutUser(HttpContext);

            return Redirect(referer);
        }
    }
}
