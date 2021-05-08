using Edurem.Models;
using Edurem.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Edurem.Filters
{
    public class OnlyGroupAdminFilter : Attribute, IActionFilter
    {
        IGroupService GroupService { get; set; }
        public OnlyGroupAdminFilter(IGroupService groupService)
        {
            GroupService = groupService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = int.Parse(context.HttpContext.User.GetClaim(ClaimKey.Id));

            var groupId = int.Parse(context.ActionArguments["id"].ToString());

            var role = GroupService.GetMembers(groupId).Result.First(member => member.UserId == userId).RoleInGroup;

            if (!role.Equals(RoleInGroup.ADMIN))
                context.Result = new RedirectToActionResult("GroupPosts", "Group", new { id = groupId });
        }
    }
}
