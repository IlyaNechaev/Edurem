using Edurem.Data;
using Edurem.Data.Repositories;
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
        public async Task<IActionResult> Settings()
        {
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);
            var userNotificationOptions = await UserService.GetUserNotificationOptions(authenticatedUser);

            var settings = new SettingsViewModel();

            settings.Notifications = userNotificationOptions;

            var accountViewModel = new AccountViewModel<SettingsViewModel>() { ViewModel = settings, CurrentUser = authenticatedUser };

            return View(accountViewModel);
        }


        [Route("settings")]
        [HttpPost]
        public async Task<IActionResult> Settings(AccountViewModel<SettingsViewModel> accountViewModel)
        {
            TryValidateModel(accountViewModel.CurrentUser);
            if (!ModelState.IsValid)
            {
                return View(accountViewModel);
            }
            else
            {
                try
                {
                    await UserService.UpdateUser(accountViewModel.CurrentUser);
                }
                catch (DatabaseServiceException)
                {
                    ModelState.AddModelError("", "Не удалось изменить данные. Попробуйте повторить опреацию позже");
                    return View(accountViewModel);
                }
            }
            return View(accountViewModel);
        }

        [Route("groups")]
        [HttpGet]
        public async Task<IActionResult> Groups([FromServices] IGroupService groupService)
        {
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);
            var groups = await groupService.GetUserGroups(authenticatedUser);

            var groupsListViewModel = new GroupsListViewModel(groups);

            var accountViewModel = new AccountViewModel<GroupsListViewModel>() { ViewModel = groupsListViewModel, CurrentUser = authenticatedUser };

            return View(accountViewModel);
        }

        [Route("groups/create")]
        [HttpGet]
        public IActionResult CreateGroup([FromServices] IGroupService groupService)
        {

            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);

            var accountViewModel = new AccountViewModel<GroupCreationEditModel>() { ViewModel = new GroupCreationEditModel(), CurrentUser = authenticatedUser };

            return View(accountViewModel);
        }

        [Route("groups/create")]
        [HttpPost]
        public async Task<IActionResult> CreateGroup(GroupCreationEditModel creationViewModel,
                                         [FromServices] IGroupService groupService)
        {
            var newGroup = creationViewModel.ToGroup();
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);

            await groupService.CreateGroup(newGroup, authenticatedUser);

            return RedirectToAction("Groups");
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
