using Edurem.Data;
using Edurem.Models;
using Edurem.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Edurem.Services
{
    public class GroupService : IGroupService
    {
        IRepositoryFactory RepositoryFactory { get; init; }
        IEmailService EmailService { get; init; }
        IConfiguration Configuration { get; init; }
        IFileService FileService { get; init; }
        IHttpContextAccessor HttpContextAccessor { get; init; }
        ISecurityService SecurityService { get; init; }

        public GroupService(
            IRepositoryFactory repositoryFactory,
            IEmailService emailService,
            IConfiguration configuration,
            IFileService fileService,
            ISecurityService securityService,
            IHttpContextAccessor httpContextAccessor)
        {
            RepositoryFactory = repositoryFactory;
            EmailService = emailService;
            Configuration = configuration;
            FileService = fileService;
            HttpContextAccessor = httpContextAccessor;
            SecurityService = securityService;
        }

        public async Task<List<(Group, RoleInGroup)>> GetUserGroups(User user)
        {
            return await GetUserGroups(user.Id);
        }

        public async Task<List<(Group, RoleInGroup)>> GetUserGroups(int userId)
        {
            var GroupRepository = RepositoryFactory.GetRepository<Group>();
            // Находим группы, в которых состоит пользователь
            var groups = await GroupRepository.Find(group => group.Members.Any(gm => gm.UserId == userId), nameof(Group.Subject), nameof(Group.Members));

            return groups.Select(group => (group, group.Members.First(gm => gm.UserId == userId).RoleInGroup)).ToList();
        }

        public async Task CreateGroup(Group group, User creator)
        {
            var GroupRepository = RepositoryFactory.GetRepository<Group>();
            var GroupMemberRepository = RepositoryFactory.GetRepository<GroupMember>();

            await GroupRepository.Add(group);

            await GroupMemberRepository.Add(new GroupMember { GroupId = group.Id, UserId = creator.Id, RoleInGroup = RoleInGroup.ADMIN });
        }

        public async Task AddSubject(string subjectName, User user)
        {
            await AddSubject(subjectName, user.Id);
        }
        public async Task AddSubject(string subjectName, int userId)
        {
            var SubjectRepository = RepositoryFactory.GetRepository<Subject>();

            if ((await SubjectRepository.Get(subject => subject.AuthorId == userId && subject.Name.Equals(subjectName))) is null)
            {
                var subject = new Subject() { AuthorId = userId, Name = subjectName };

                await SubjectRepository.Add(subject);
            }
        }

        public async Task<List<Subject>> GetUserSubjects(User user)
        {
            return await GetUserSubjects(user.Id);
        }

        public async Task<List<Subject>> GetUserSubjects(int userId)
        {
            var SubjectRepository = RepositoryFactory.GetRepository<Subject>();

            return (await SubjectRepository.Find(subject => subject.AuthorId == userId)).ToList();
        }

        public async Task<Group> GetGroup(int groupId)
        {
            var GroupRepository = RepositoryFactory.GetRepository<Group>();

            return await GroupRepository.Get(group => group.Id == groupId, nameof(Group.Subject), nameof(Group.Members));
        }

        public async Task<List<PostModel>> GetGroupPosts(int groupId, int startIndex = 0, int postsCount = 1)
        {
            var GroupPostRepository = RepositoryFactory.GetRepository<GroupPost>();
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();

            // Находим идентификаторы публикаций для данной группы
            var postsId = (await GroupPostRepository.Find(gp => gp.GroupId == groupId)).Select(gp => gp.PostId).ToList();

            var posts = (await PostRepository.Find(post => postsId.Contains(post.Id), nameof(PostModel.AttachedFiles), nameof(PostModel.Author)));

            // Находим последние несколько постов (postsCount), начиная со startIndex
            return posts?
                .OrderBy(post => post.PublicationDate)?
                .Reverse()?
                .Skip(startIndex)?
                .Take(postsCount)?
                .ToList();
        }

        public async Task<List<GroupMember>> GetMembers(int groupId)
        {
            var members = (await RepositoryFactory
                .GetRepository<GroupMember>()
                .Find(gm => gm.GroupId == groupId, nameof(GroupMember.User), nameof(GroupMember.Group)))
                .ToList();

            return members;
        }

        public async Task Invite(int groupId, List<string> emailsToInvite)
        {
            var groupName = (await GetGroup(groupId)).Name;

            var message = File.ReadAllText(
                FileService.GetFullPath(
                    Configuration.GetFilePath("InvitationPattern")
                    )
                );

            Parallel.ForEach(emailsToInvite,
                async email =>
                {
                    var inviteCode = SecurityService.Encrypt($"group_id={groupId}&email={email}", "edurem")
                    .Replace("+", "&")
                    .Replace("/", "&&");
                    var request = HttpContextAccessor.HttpContext.Request;

                    var link = $"{request.Scheme}://{request.Host}/group/join/{inviteCode}";

                    message = message
                    .Replace("@group_name", groupName)
                    .Replace("@link", link);

                    var options = new EmailOptions
                    {
                        Text = message,
                        Subject = "Подтверждение Email",
                        Sender = ("ilia.nechaeff@yandex.ru", "Edurem"),
                        Receivers = new() { (email, "") },
                        SmtpServer = ("smtp.yandex.ru", 25, false),
                        AuthInfo = ("ilia.nechaeff@yandex.ru", "02081956Qw")
                    };

                    await EmailService.SendEmailAsync(options);
                });
        }

        public async Task<(bool HasErrors, int GroupId, int UserId, string Email)> IsInvited(string code)
        {
            code = code.Replace("&&", "/").Replace("&", "+");

            var decryptCode = new string[2];
            var groupId = string.Empty;
            var email = string.Empty;
            try
            {
                decryptCode = SecurityService.Decrypt(code, "edurem").Split("&");
                groupId = decryptCode[0].Split("=")[1];
                email = decryptCode[1].Split("=")[1];
            }
            catch (Exception)
            {
                return (true, 0, 0, string.Empty);
            }

            var groupToJoin = await RepositoryFactory.GetRepository<Group>().Get(group => group.Id == int.Parse(groupId));

            if (groupToJoin is null) return (true, 0, 0, string.Empty);

            var userToJoin = await RepositoryFactory.GetRepository<User>().Get(user => user.Email == email);

            return (false, groupToJoin.Id, userToJoin?.Id ?? 0, email);
        }

        public async Task JoinGroup(int userId, int groupId)
        {
            var GroupMemberRepository = RepositoryFactory.GetRepository<GroupMember>();

            if (GroupMemberRepository.Get(gm => (gm.UserId == userId && gm.GroupId == groupId)) is not null)
                return;

            var groupMember = new GroupMember
            {
                GroupId = groupId,
                UserId = userId,
                RoleInGroup = RoleInGroup.MEMBER
            };

            await RepositoryFactory.GetRepository<GroupMember>().Add(groupMember);
        }
    }
}
