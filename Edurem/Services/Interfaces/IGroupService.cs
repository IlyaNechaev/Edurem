using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.Services
{
    public interface IGroupService
    {
        /// <summary>
        /// Возвращает группы в которых состоит пользователь
        /// </summary>
        public Task<List<(Group Group, RoleInGroup UserRole)>> GetUserGroups(User user);

        /// <summary>
        /// Возвращает группы в которых состоит пользователь
        /// </summary>
        public Task<List<(Group Group, RoleInGroup UserRole)>> GetUserGroups(int userId);

        public Task CreateGroup(Group group, User creator);

        public Task AddSubject(string subjectName, User user);
        public Task AddSubject(string subjectName, int userId);

        public Task<List<Subject>> GetUserSubjects(User user);
        public Task<List<Subject>> GetUserSubjects(int userId);

        public Task<Group> GetGroup(int groupId);

        public Task<List<PostModel>> GetGroupPosts(int groupId, int startIndex = 0, int postsCount = 1);        

        public Task<IEnumerable<GroupMember>> GetMembers(int groupId);

        public Task Invite(int groupId, List<string> emailsToInvite);

        public Task<(bool HasErrors, int GroupId, int UserId, string Email)> IsInvited(string code);

        public Task JoinGroup(int userId, int groupId);
    }
}
