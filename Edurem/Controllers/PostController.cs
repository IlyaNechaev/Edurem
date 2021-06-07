using Edurem.Data;
using Edurem.Models;
using Edurem.Services;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Controllers
{
    [Route("post")]
    [Authorize(Policy = "AuthenticatedOnly")]
    public class PostController : Controller
    {
        IRepositoryFactory RepositoryFactory { get; init; }
        IUserService UserService { get; init; }

        public PostController(IRepositoryFactory repositoryFactory, 
            IUserService userService)
        {
            RepositoryFactory = repositoryFactory;
            UserService = userService;
        }

        [Route("{postId}")]
        [HttpGet]
        public async Task<IActionResult> Index(int postId)
        {
            var postRepository = RepositoryFactory.GetRepository<PostModel>();
            var fileRepository = RepositoryFactory.GetRepository<FileModel>();

            var postModel = await postRepository.Get(postModel => postModel.Id == postId, nameof(PostModel.AttachedFiles));
            var postFiles = await fileRepository.Find(file => postModel.AttachedFiles.Select(af => af.Id).Contains(file.Id));

            PostViewModel postViewModel = new();
            postViewModel.FromPostModel(postModel, postFiles);

            return View(postViewModel);
        }

        [Route("{postId}/test")]
        [HttpGet]
        public async Task<IActionResult> Test(int postId, int userId)
        {
            var PostModelRepository = RepositoryFactory.GetRepository<PostModel>();

            var post = await PostModelRepository.Get(post => post.Id == postId);

            return View(post);
        }


        [Route("{postId}/edit")]
        [HttpGet]
        public IActionResult EditPost(int postId, [FromQuery] int groupId)
        {
            return View((groupId, postId));
        }
    }
}
