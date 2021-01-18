using Edurem.Data;
using Edurem.Models;
using Edurem.Services;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Controllers
{
    [Route("account")]
    [Authorize(Policy = "AuthenticatedOnly")]
    public class AccountController : Controller
    {
        IUserService UserService;

        public AccountController(IUserService userService)
        {
            UserService = userService;
        }

        public IActionResult Index()
        {
            // Получаем статус пользователя из Claims
            var userStatus = (Status)Enum.Parse(typeof(Status), HttpContext.User.Claims.First(claim => claim.Type == "Status").Value);

            switch (userStatus)
            {
                case Status.REGISTERED:
                    return RedirectToAction("Index");
                case Status.ACTIVATED:
                    return RedirectToAction("Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [Route("home")]
        public IActionResult Home([FromServices] IConfiguration config)
        {
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);
            return View(authenticatedUser);
        }

        [Route("settings")]
        [HttpGet]
        public IActionResult Settings()
        {
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);
            var userNotificationOptions = UserService.GetUserNotificationOptions(authenticatedUser);

            var settings = new SettingsViewModel();

            settings.Profile = authenticatedUser;
            settings.Notifications = userNotificationOptions;

            return View(settings);
        }


        [Route("settings")]
        [HttpPost]
        public async Task<IActionResult> Settings(User userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }
            else
            {
                try
                {
                    await UserService.UpdateUser(userModel);
                }
                catch (DatabaseServiceException)
                {
                    ModelState.AddModelError("", "Не удалось изменить данные. Попробуйте повторить опреацию позже");
                    return View(userModel);
                }
            }
            return View(userModel);
        }

        [Route("verifyEmail")]
        [HttpGet]
        public IActionResult VerifyEmail()
        {
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);
            return View(authenticatedUser);
        }

        [Route("accessDenied")]
        [Authorize(Policy = "AccessDenied")]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            var claims = HttpContext.User.Claims.ToList();
            var cause = "";

            foreach (var claim in claims)
            {
                if ((claim.Type, claim.Value) == ("Status", "REGISTERED"))
                    cause = "Registered";
                else if ((claim.Type, claim.Value) == ("Status", "BLOCKED"))
                    cause = "Blocked";
            }
            return Content($"Access denied {cause}");
        }
    }
}
