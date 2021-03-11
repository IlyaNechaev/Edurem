using Edurem.Services;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;
using Edurem.Data;

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
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);
            var group = await GroupService.GetGroup(id);
            var groupView = new GroupViewModel(group, group.Members.First(gm => gm.UserId == authenticatedUser.Id).RoleInGroup, group.Subject);

            var accountViewModel = new AccountViewModel<GroupViewModel>() { CurrentUser = authenticatedUser, ViewModel = groupView };

            return View("GroupPosts", accountViewModel);
        }

        [Route("{id}/createPost")]
        [HttpGet]
        public async Task<IActionResult> CreatePost(int id)
        {
            var authenticatedUser = UserService.GetAuthenticatedUser(HttpContext);
            var group = await RepositoryFactory.GetRepository<Group>().Get(group => group.Id == id);

            var accountViewModel = new AccountViewModel<Group>() { CurrentUser = authenticatedUser, ViewModel = group };

            return View(accountViewModel);
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
