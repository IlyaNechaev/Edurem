using Edurem.Data;
using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class GroupService : IGroupService
    {
        IRepositoryFactory RepositoryFactory { get; init; }

        public GroupService([FromServices] IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        public async Task<List<(Group, RoleInGroup)>> GetUserGroups(User user)
        {
            var GroupRepository = RepositoryFactory.GetRepository<Group>();
            // Находим группы, в которых состоит пользователь
            var groups = await GroupRepository.Find(group => group.Members.Any(gm => gm.UserId == user.Id), nameof(Group.Subject), nameof(Group.Members));

            return groups.Select(group => (group, group.Members.First(gm => gm.UserId == user.Id).RoleInGroup)).ToList();
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
            var SubjectRepository = RepositoryFactory.GetRepository<Subject>();

            var subject = new Subject() { AuthorId = user.Id, Name = subjectName };

            await SubjectRepository.Add(subject);
        }

        public async Task<List<Subject>> GetUserSubjects(User user)
        {
            var SubjectRepository = RepositoryFactory.GetRepository<Subject>();

            return (await SubjectRepository.Find(subject => subject.AuthorId == user.Id)).ToList();
        }

        public async Task<Group> GetGroup(int groupId)
        {
            var GroupRepository = RepositoryFactory.GetRepository<Group>();

            return await GroupRepository.Get(group => group.Id == groupId, nameof(Group.Subject), nameof(Group.Members));
        }

        
        public async Task<List<PostModel>> GetGroupPosts(int groupId, int startIndex = 0, int postsCount = 1)
        {
            var GroupPostRepository = RepositoryFactory.GetRepository<GroupPost>();

            // 
            var groupPosts = (await GroupPostRepository.Find(gp => gp.GroupId == groupId, nameof(GroupPost.Post)));

            // Находим последние несколько постов (postsCount), начиная со startIndex
            return groupPosts?
                .Select(gp => gp.Post)?
                .OrderBy(post => post.PublicationDate)?
                .Reverse()?
                .Skip(startIndex)?
                .Take(postsCount)?
                .ToList();
        }
    }
}
