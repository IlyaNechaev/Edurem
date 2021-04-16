﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Edurem.Models;
using Microsoft.AspNetCore.Hosting;
using Edurem.Data;
using System.IO;
using Edurem.Services;

namespace Edurem.Controllers
{

    [Route("home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IUserService userService, IDbService dbService)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index([FromServices] IDockerService dockerService, [FromServices] IWebHostEnvironment appEnvironment)
        {
            /*
            var contextPath = Path.Combine(appEnvironment.WebRootPath, "file_system\\tests", $"post_1");
            var parameters = new Docker.DotNet.Models.ImageBuildParameters
            {
                Dockerfile = Path.Combine(appEnvironment.WebRootPath, "file_system\\tests", $"post_1", $"user_1", "Dockerfile"),
                Tags = new List<string>() { "test_1_1" }
            };

            dockerService.CreateImage(contextPath, parameters);*/
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("FAQ")]
        public IActionResult Faq()
        {
            return View();
        }

        public IActionResult Default()
        {
            return Redirect("home");
        }
    }
}
