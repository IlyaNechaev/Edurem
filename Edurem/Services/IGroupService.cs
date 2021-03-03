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

        // Возвращает группы в которых состоит пользователь
        public Task<List<(Group Group, RoleInGroup UserRole)>> GetUserGroups(User user);

        public Task CreateGroup(Group group, User creator);

        public Task AddSubject(string subjectName, User user);

        public Task<List<Subject>> GetUserSubjects(User user);

        public Task<Group> GetGroup(int groupId);

        public Task<List<PostModel>> GetGroupPosts(int groupId, int startIndex = 0, int postsCount = 1);

        public Task CreatePost(PostModel post, List<FileModel> files = null );
    }
}
