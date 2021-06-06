using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Edurem.Data;
using Edurem.Extensions;
using Edurem.Models;
using Edurem.Services;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Edurem.Controllers
{
    public class AdministrationController : Controller
    {
        IUserService UserService;
        IRepositoryFactory RepositoryFactory;

        public AdministrationController(
            [FromServices] IUserService userService,
            [FromServices] IRepositoryFactory repositoryFactory)
        {
            UserService = userService;
            RepositoryFactory = repositoryFactory;
        }

        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            // Если пользователь авторизован
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Home", "Account");

            var registerModel = new RegisterEditModel();
            var redirect = string.Empty;
            var email = string.Empty;

            if (TempData.ContainsKey("EmailToRegister"))
            {
                email = TempData["EmailToRegister"].ToString();
            }

            if (TempData.ContainsKey("RedirectedFrom"))
            {
                redirect = TempData["RedirectedFrom"].ToString();
            }

            registerModel.Redirect = redirect;
            registerModel.Email = email;

            return View(registerModel);
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterEditModel model)
        {
            // Присутствуют ли в моделе ошибки
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Регистрация пользователя
            var registerResult = await UserService.RegisterUser(model);

            // Если во время регистрации были получены ошибки
            if (registerResult.HasErrors)
            {
                // Добавить все ошибки в ModelState для передачи в представление
                registerResult.ErrorMessages.ForEach(error => ModelState.AddModelError(error.Key, error.Message));
                return View(model);
            }

            await UserService.SignInUser(model.Login, model.Password, HttpContext);

            if (!string.IsNullOrEmpty(model.Redirect))
            {
                return Redirect(model.Redirect);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            // Если пользователь авторизован
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Home", "Account");

            var loginModel = new LoginEditModel();
            var redirect = string.Empty;

            if (TempData.ContainsKey("RedirectedFrom"))
            {
                redirect = TempData["RedirectedFrom"].ToString();
            }
            else
            {
                try
                {
                    var referer = HttpContext.Request.Headers["Referer"];
                    redirect = new Uri(referer.Last()).LocalPath;
                }
                catch (Exception) { }
            }

            loginModel.Redirect = redirect;

            return View(loginModel);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginEditModel model)
        {
            // Проверяем, присутствуют ли в моделе ошибки
            if (ModelState.IsValid)
            {
                var result = await UserService.SignInUser(model.Login, model.Password, HttpContext);

                // Если во время аутентификации были получены ошибки
                if (result.HasErrors)
                {
                    // Добавить все ошибки в ModelState для передач в представление
                    result.ErrorMessages.ForEach(error => ModelState.AddModelError(error.Key, error.Message));
                    return View(model);
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.Redirect))
                    {
                        return Redirect(model.Redirect);
                    }
                    else
                    {
                        return RedirectToAction("Home", "Account");
                    }
                }
            }

            return View(model);
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Адрес, с которого был произведен запрос
            var referer = new Uri(Request.Headers["Referer"].Last()).LocalPath;

            await UserService.LogoutUser(HttpContext);

            return Redirect(referer);
        }

        [Route("/files/download")]
        [Authorize(Policy = "AuthenticatedOnly")]
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

        [Route("/code/download")]
        [Authorize(Policy = "AuthenticatedOnly")]
        [HttpGet]
        public async Task<IActionResult> DownloadCodeFile(int postId, int userId,
            [FromServices] IFileService fileService,
            [FromServices] IConfiguration configuration,
            [FromServices] IWebHostEnvironment appEnvironment,
            [FromServices] IUserService userService,
            [FromServices] IPostService postService)
        {
            var filePaths = Directory.GetFiles(Path.Combine(appEnvironment.WebRootPath, configuration.GetDirectoryPath().ForUserSolution(postId.ToString(), userId.ToString())));

            var zipItems = new List<ZipItem>();

            foreach (var file in filePaths)
            {
                zipItems.Add(new ZipItem(Path.GetFileName(file), System.IO.File.OpenRead(file)));
            }

            var UserRepository = RepositoryFactory.GetRepository<User>();

            var postTitle = (await postService.GetPost(postId)).Title;
            var user = await UserRepository.Get(u => u.Id == userId);

            var userName = $"{user.Surname} {user.Name}";

            return File(fileService.ZipFiles(zipItems), "application/octet-stream", $"{postTitle}. {userName}.zip");
        }


        [Route("/results/download")]
        [HttpGet]
        public async Task<IActionResult> DownloadStatisticsTable(
            int groupId,
            string posts,
            [FromServices] IGroupService groupService,
            [FromServices] ICodeTestService testService,
            [FromServices] IPostService postService)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var tableStream = new MemoryStream();
            using (var package = new ExcelPackage(tableStream))
            {

                List<int> postIds = posts.Split("p").Select(id => int.Parse(id)).ToList();

                foreach (var postId in postIds)
                {
                    var testResults = GetTestResults(postId)
                        .Select(results =>
                        {
                            if (results.CountOfTests > 0)
                            {
                                return new
                                {
                                    Name = $"{results.User.Surname} {results.User.Name}",
                                    Progress = results.GetProgress(),
                                    LastTestTime = (results.LastTestTime.IsTested ? results.LastTestTime.Time.ToString() : "-"),
                                    Output = results.Results.Select(result => (result.TestName, ParseHtml(result.ResultHtml)))
                                };
                            }
                            else
                            {

                                return new
                                {
                                    Name = $"{results.User.Surname} {results.User.Name}",
                                    Progress = results.GetProgress(),
                                    LastTestTime = (results.LastTestTime.IsTested ? results.LastTestTime.Time.ToString() : "-"),
                                    Output = results.Results.Select(result => (result.TestName, ParseHtml(result.ResultHtml)))
                                };
                            }
                        }).ToList();

                    var postTitle = postService.GetPost(postId).Result.Title;

                    var worksheet = package.Workbook.Worksheets.Add(postTitle);

                    worksheet.Cells[1, 1].Value = "Имя";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 2].Value = "Прогресс, %";
                    worksheet.Cells[1, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 3].Value = "Дата последнего тестирования";
                    worksheet.Cells[1, 3].Style.Font.Bold = true;
                    worksheet.Cells[1, 4].Value = "Результат";
                    worksheet.Cells[1, 4].Style.Font.Bold = true;

                    for (int i = 2; i <= testResults.Count + 1; i++)
                    {
                        worksheet.Cells[i, 1].Value = testResults[i - 2].Name;
                        worksheet.Cells[i, 2].Value = testResults[i - 2].Progress;
                        worksheet.Cells[i, 3].Value = testResults[i - 2].LastTestTime;
                        worksheet.Cells[i, 4].Value = testResults[i - 2].Output;
                    }

                    worksheet.Cells.AutoFitColumns();

                    await package.SaveAsync();
                }

                var groupTitle = groupService.GetGroup(groupId).Result.Name;

                package.Stream.Position = 0;

                var result = File(package.Stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{groupTitle}.xlsx");

                return result;
            }

            List<TestResultsView> GetTestResults(int postId)
            {
                var usersInGroup = groupService.GetMembers(groupId)
                    .Result
                    .Where(user => user.RoleInGroup.Equals(RoleInGroup.MEMBER));

                var testResultViews = postService.GetTestResults(postId)
                    .Result
                    .Where(testResult => usersInGroup.Any(user => user.UserId == testResult.UserId))
                    .Select(testResult => new TestResultsView(testResult, testService.BuildTestResults(testResult)))
                    .ToList();

                foreach (var user in usersInGroup)
                {
                    if (!testResultViews.Any(result => result.User.Id == user.UserId))
                    {
                        var resultView = new TestResultsView
                        {
                            CountOfCompletedTests = 0,
                            CountOfTests = 0,
                            HasError = false,
                            Results = new(),
                            LastTestTime = (false, DateTime.Now),
                            User = new User
                            {
                                Name = user.User.Name,
                                Surname = user.User.Surname
                            }
                        };
                        testResultViews.Add(resultView);
                    }
                }

                return testResultViews;
            }
        }

        private string ParseHtml(string html)
        {
            var text = string.Join("\n", html.Split("<").Select(line => line.Split(">").Last()));

            return text;
        }
    }
}