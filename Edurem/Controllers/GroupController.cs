using Edurem.Services;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.Controllers
{
    [Route("group")]
    public class GroupController : Controller
    {
        IUserService UserService { get; init; }
        IGroupService GroupService { get; init; }

        public GroupController([FromServices] IUserService userService,
                                [FromServices] IGroupService groupService)
        {
            UserService = userService;
            GroupService = groupService;
        }

        [Route("{id}")]
        public async Task<IActionResult> GroupPosts(int id)
        {
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);
            var group = await GroupService.GetGroup(id);
            var groupView = new GroupViewModel(group, group.Members.First(gm => gm.UserId == authenticatedUser.Id).RoleInGroup, group.Subject);

            var accountViewModel = new AccountViewModel<GroupViewModel>() { CurrentUser = authenticatedUser, ViewModel = groupView };

            return View("GroupPosts", accountViewModel);
        }
    }
}
