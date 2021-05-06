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
        IUserService UserService { get; set; }

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
        public IActionResult Home()
        {
            return View();
        }

        [Route("settings")]
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var currentUserId = int.Parse(HttpContext.User.GetClaim(ClaimKey.Id));
            var userNotificationOptions = await UserService.GetUserNotificationOptions(currentUserId);

            var settingsViewModel = new SettingsViewModel();

            settingsViewModel.Notifications = userNotificationOptions;

            settingsViewModel.User = await UserService.GetAuthenticatedUser(HttpContext);

            return View(settingsViewModel);
        }


        [Route("settings")]
        [HttpPost]
        public async Task<IActionResult> Settings(SettingsViewModel settingsViewModel)
        {
            TryValidateModel(settingsViewModel.User);
            if (!ModelState.IsValid)
            {
                return View(settingsViewModel);
            }
            else
            {
                try
                {
                    await UserService.UpdateUser(settingsViewModel.User);
                }
                catch (DatabaseServiceException)
                {
                    ModelState.AddModelError("", "Не удалось изменить данные. Попробуйте повторить опреацию позже");
                    return View(settingsViewModel);
                }
            }
            return View(settingsViewModel);
        }

        [Route("groups")]
        [HttpGet]
        public IActionResult Groups([FromServices] IGroupService groupService)
        {
            var accountViewModel = new AccountViewModel(HttpContext);

            return View(accountViewModel);
        }

        [Route("groups/create")]
        [HttpGet]
        public IActionResult CreateGroup([FromServices] IGroupService groupService)
        {
            return View(new GroupCreationEditModel());
        }

        [Route("groups/create")]
        [HttpPost]
        public async Task<IActionResult> CreateGroup(GroupCreationEditModel groupCreationModel,
                                         [FromServices] IGroupService groupService)
        {
            // Если в модели создания группы имеет ошибка
            if (!TryValidateModel(groupCreationModel))
            {
                return View(groupCreationModel);
            }

            var newGroup = groupCreationModel.ToGroup();
            var authenticatedUser = await UserService.GetAuthenticatedUser(HttpContext);

            await groupService.CreateGroup(newGroup, authenticatedUser);

            return RedirectToAction("Groups");
        }

        [Route("verifyEmail")]
        [HttpGet]
        public async Task<IActionResult> VerifyEmail()
        {
            var authenticatedUser = await UserService.GetAuthenticatedUser(HttpContext);
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
                cause = (claim.Type, claim.Value) switch
                {
                    ("Status", "REGISTERED") => "Registered",
                    ("Status", "BLOCKED") => "Blocked",
                    _ => cause
                };
            }
            return Content($"Access denied {cause}");
        }
    }
}
