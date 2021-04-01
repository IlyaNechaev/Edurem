using Edurem.Data;
using Edurem.Models;
using Edurem.Services;
using Edurem.ViewModels;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Controllers
{
    [Route("post")]
    public class PostController : Controller
    {
        CurrentUserViewModel currentUser { get; set; }
        IRepositoryFactory RepositoryFactory { get; init; }
        IUserService UserService { get; init; }

        public PostController(IRepositoryFactory repositoryFactory, 
            IUserService userService)
        {
            RepositoryFactory = repositoryFactory;
            UserService = userService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            GetCurrentUser();

            var postRepository = RepositoryFactory.GetRepository<PostModel>();
            var fileRepository = RepositoryFactory.GetRepository<FileModel>();

            var postModel = await postRepository.Get(postModel => postModel.Id == id, nameof(PostModel.AttachedFiles));
            var postFiles = (await fileRepository.Find(file => postModel.AttachedFiles.Select(af => af.FileId).Contains(file.Id))).ToList();

            PostViewModel postViewModel = new();
            postViewModel.FromPostModel(postModel, postFiles);

            var viewModel = new AccountViewModel<PostViewModel> { CurrentUser = currentUser, ViewModel = postViewModel };

            return View(viewModel);
        }

        private void GetCurrentUser()
        {
            currentUser = currentUser ?? new CurrentUserViewModel
            {
                Id = int.Parse(User.FindFirst(ClaimKey.Id).Value),
                Name = User.FindFirst(ClaimKey.Name).Value,
                Surname = User.FindFirst(ClaimKey.Surname).Value
            };
        }
    }
}
