using Edurem.Services;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;
using Edurem.Data;
using Microsoft.AspNetCore.Authorization;

namespace Edurem.Controllers
{
    [Route("group")]
    public class GroupController : Controller
    {
        IUserService UserService { get; init; }
        IGroupService GroupService { get; init; }
        IRepositoryFactory RepositoryFactory { get; init; }

        public GroupController([FromServices] IUserService userService,
                                [FromServices] IGroupService groupService,
                                [FromServices] IRepositoryFactory repositoryFactory) 
        {
            UserService = userService;
            GroupService = groupService;
            RepositoryFactory = repositoryFactory;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GroupPosts(int id)
        {
            var currentUserId = int.Parse(HttpContext.User.GetClaim(ClaimKey.Id));

            var group = await GroupService.GetGroup(id);
            var postsCount = (await GroupService.GetGroupPosts(id, postsCount: int.MaxValue)).Count();

            var groupView = new GroupViewModel(group, group.Members.First(gm => gm.UserId == currentUserId).RoleInGroup, group.Subject);
            groupView.PostsCount = postsCount;

            return View("GroupPosts", groupView);
        }

        [Route("{id}/createPost")]
        [HttpGet]
        public async Task<IActionResult> CreatePost(int id)
        {
            var group = await RepositoryFactory.GetRepository<Group>().Get(group => group.Id == id);

            return View(group);
        }

        [Route("{id}/invite")]
        [HttpGet]
        public async Task<IActionResult> Invite(int id)
        {
            var currentUserId = int.Parse(HttpContext.User.GetClaim(ClaimKey.Id));

            var group = await GroupService.GetGroup(id);
            var postsCount = (await GroupService.GetGroupPosts(id, postsCount: int.MaxValue)).Count();

            var groupView = new GroupViewModel(group, group.Members.First(gm => gm.UserId == currentUserId).RoleInGroup, group.Subject);
            groupView.PostsCount = postsCount;

            return View(groupView);
        }

        [Route("{id}/invite")]
        [HttpPost]
        public async Task<IActionResult> Invite(int id, string emailsToInvite)
        {
            var emails = emailsToInvite
                .Split(";")
                .Select(email => email.Trim())
                .ToList();

            await GroupService.Invite(id, emails);

            return RedirectToAction(nameof(GroupPosts), new { id = id });
        }

        [Route("join/{code}")]
        [HttpGet]
        public async Task<IActionResult> Join(string code, 
            [FromServices] ICookieService cookieService)
        {
            var isInvited = await GroupService.IsInvited(code);

            if (isInvited.HasErrors)
                return RedirectToAction("Index", "Home");

            // Если запрос производит авторизованный пользователь
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var id = int.Parse(HttpContext.User.GetClaim(ClaimKey.Id));

                if (id == isInvited.UserId)
                {
                    await GroupService.JoinGroup(isInvited.UserId, isInvited.GroupId);

                    return RedirectToAction("GroupPosts", new { id = isInvited.GroupId });
                }
                else
                {
                    return RedirectToAction("Index", "Account");
                }
            }
            else
            {
                if (isInvited.UserId == 0)
                {
                    return RedirectToAction("Register", "Administration", new { email = isInvited.Email });
                }
                else
                {
                    return RedirectToAction("Login", "Administration", new { email = isInvited.Email });
                }
            }
        }

        [Route("/files/download")]
        [HttpGet]
        public async Task<IActionResult> DownloadFile(int fileId, [FromServices] IFileService FileService)
        {
            Dictionary<string, string> mimeTypes = new()
            {
                { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { "doc", "application/msword" },
                { "ppt", "application/vnd.ms-powerpoint" },
                { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { "xls", "application/vnd.ms-excel" },
                { "jpg", "image/jpeg" },
                { "jpeg", "image/jpeg" },
                { "png", "image/png" },
                { "pdf", "application/pdf" }
            };

            var FileRepository = RepositoryFactory.GetRepository<FileModel>();

            var file = await FileRepository.Get(file => file.Id == fileId);

            return File(FileService.OpenFile(file), mimeTypes[file.Name.Split(".").Last()], file.Name);
        }
    }
}
